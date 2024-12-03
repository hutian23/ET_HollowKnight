using System;

namespace ET.Client
{
    [FriendOf(typeof (InputWait))]
    public static class InputWaitSystem
    {
        public class InputWaitFrameUpdateSystem: FrameUpdateSystem<InputWait>
        {
            protected override void FrameUpdate(InputWait self)
            {
                self.Ops = BBInputComponent.Instance.CheckInput();  
                //1. 启动输入检测协程 
                self.StartInputHandler();
                //2. 更新按键历史
                self.UpdateKeyHistory(self.Ops);
                //3. 添加缓冲
                self.Notify(self.Ops);
            }
        }

        public static void Init(this InputWait self)
        {
            self.Ops = 0;
            
            self.Token?.Cancel();
            self.tcss.ForEach(tcs => tcs.Recycle());
            self.tcss.Clear();
            self.Token = new ETCancellationToken();
            self.InputWaitRunQueue.Clear();

            self.BufferFlag = false;
            self.BufferDict.Clear();
            self.PressedDict.Clear();
            self.RegistKeyHistory();
        }

        #region KeyHistory

        private static void RegistKeyHistory(this InputWait self)
        {
            self.PressedDict.Add(BBOperaType.X, long.MaxValue);
            self.PressedDict.Add(BBOperaType.A, long.MaxValue);
            self.PressedDict.Add(BBOperaType.Y, long.MaxValue);
            self.PressedDict.Add(BBOperaType.B, long.MaxValue);
            self.PressedDict.Add(BBOperaType.RT, long.MaxValue);
            self.PressedDict.Add(BBOperaType.RB, long.MaxValue);
            self.PressedDict.Add(BBOperaType.LB, long.MaxValue);
            self.PressedDict.Add(BBOperaType.LT, long.MaxValue);
        }

        private static void UpdateKeyHistory(this InputWait self, long ops)
        {
            self.HandleKeyInput(ops, BBOperaType.X);
            self.HandleKeyInput(ops, BBOperaType.A);
            self.HandleKeyInput(ops, BBOperaType.Y);
            self.HandleKeyInput(ops, BBOperaType.B);
            self.HandleKeyInput(ops, BBOperaType.RB);
            self.HandleKeyInput(ops, BBOperaType.RT);
            self.HandleKeyInput(ops, BBOperaType.LB);
            self.HandleKeyInput(ops, BBOperaType.LT);
        }
        
        private static void HandleKeyInput(this InputWait self, long ops, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            bool ret = (ops & operaType) != 0;
            //更新按键是在哪一帧按下的
            if (ret)
            {
                if (self.PressedDict[operaType] > sceneTimer.GetNow())
                {
                    self.PressedDict[operaType] = sceneTimer.GetNow();
                }
            }
            else
            {
                self.PressedDict[operaType] = long.MaxValue;
            }
        }
        
        public static bool WasPressedThisFrame(this InputWait self, long operaType)
        {
            return self.PressedDict[operaType] == BBTimerManager.Instance.SceneTimer().GetNow();
        }

        public static bool IsPressed(this InputWait self, int operaType)
        {
            return (self.Ops & operaType) != 0;
        }

        #endregion

        //https://www.zhihu.com/question/36951135/answer/69880133
        private static void Notify(this InputWait self, long op)
        {
            for (int i = 0; i < self.tcss.Count; i++)
            {
                BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
                InputCallback callback = self.tcss[i];
                
                //1.  检测回调是否过期
                if (callback.timeOut != -1 && callback.timeOut < sceneTimer.GetNow())
                {
                    callback.SetResult(new WaitInput(){frame = 0, Error = WaitTypeError.Timeout});
                    self.tcss.Remove(callback);
                    continue;
                }
                
                //2. 当前输入不符合条件
                switch (callback.waitType)
                {
                    case FuzzyInputType.OR:
                        if ((op & callback.OP) == 0) continue;
                        break;
                    case FuzzyInputType.AND:
                        if ((op & callback.OP) != callback.OP) continue;
                        break;
                }

                //3. 执行回调的前置条件
                if (callback.checkFunc != null && !callback.checkFunc.Invoke())
                {
                    continue;
                }
                
                //4. 执行回调
                callback.SetResult(new WaitInput() { frame = sceneTimer.GetNow(), Error = WaitTypeError.Success, OP = op });
                self.tcss.Remove(callback);
            }
        }

        public static async ETTask<WaitInput> Wait(this InputWait self, long OP, int waitType, Func<bool> checkFunc = null, long timeOut = -1)
        {
            //需要检测回调是否过期
            if (timeOut != -1)
            {
                timeOut = BBTimerManager.Instance.SceneTimer().GetNow() + timeOut;
            }
            InputCallback tcs = InputCallback.Create(OP, waitType, checkFunc, timeOut);
            self.tcss.Add(tcs);

            void CancelAction()
            {
                self.tcss.Remove(tcs);
                tcs.SetResult(new WaitInput() { Error = WaitTypeError.Cancel });
                tcs.Recycle();
            }

            WaitInput ret;
            try
            {
                self.Token?.Add(CancelAction);
                ret = await tcs.Task;
            }
            finally
            {
                self.Token.Remove(CancelAction);
            }

            return ret;
        }
        
        private static void StartInputHandler(this InputWait self)
        {
            int count = self.InputWaitRunQueue.Count;
            while (count -- > 0)
            {
               string handlerName = self.InputWaitRunQueue.Dequeue();
               self.InputCheckCor(handlerName).Coroutine();
            }
        }
        
        private static async ETTask InputCheckCor(this InputWait self, string handlerName)
        {
            //1. 启动输入检测携程，等待输入
            BBInputHandler handler = DialogueDispatcherComponent.Instance.GetInputHandler(handlerName);
            InputBuffer buffer = await handler.Handle(self, self.Token);
            if (self.Token.IsCancel()) return;
                
            //2. 输入携程判定成功并且正在输入窗口中，更新技能缓冲的最大有效帧号
            if (buffer.ret is Status.Success && self.BufferFlag)
            {
                if (!self.BufferDict.ContainsKey(handler.GetBufferType()))
                {
                    self.BufferDict.TryAdd(handler.GetBufferType(), -1);
                }
                self.BufferDict[handler.GetBufferType()] = buffer.buffFrame + buffer.curFrame;
            }
            
            //3. 下一帧重新启动输入协程
            self.InputWaitRunQueue.Enqueue(handler.GetHandlerType());
        }
        
        public static bool ContainKey(this InputWait self, long op)
        {
            return (self.Ops & op) != 0;
        }

        public static void DisposeBuffer(this InputWait self)
        {
            self.BufferFlag = false;// 当前打开输入窗口
            self.BufferDict.Clear();
        }

        public static bool CheckBuffer(this InputWait self, string bufferName, long curFrame)
        {
            if (!self.BufferDict.TryGetValue(bufferName, out long timeOutFrame))
            {
                return false;
            }
            return timeOutFrame >= curFrame;
        }
    }
}