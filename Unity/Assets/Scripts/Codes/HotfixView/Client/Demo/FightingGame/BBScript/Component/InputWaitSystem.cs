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
                //1. 缓冲输入，最多可缓冲100帧指令
                self.curOP = BBInputComponent.Instance.CheckInput();
                //3. 更新按键历史
                self.UpdateKeyHistory(self.curOP);
                //4. 添加缓冲
                self.Notify(self.curOP);
            }
        }

        public static void Init(this InputWait self)
        {
            self.curOP = 0;
            
            self.Token?.Cancel();
            self.tcss.ForEach(tcs => tcs.Recycle());
            self.tcss.Clear();
            self.Token = new ETCancellationToken();

            self.BufferFlag = false;
            self.BufferDict.Clear();
            self.PressedDict.Clear();
            self.PressingDict.Clear();
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

            self.PressingDict.Add(BBOperaType.DOWNLEFT, -1);
            self.PressingDict.Add(BBOperaType.DOWN, -1);
            self.PressingDict.Add(BBOperaType.DOWNRIGHT, -1);
            self.PressingDict.Add(BBOperaType.LEFT, -1);
            self.PressingDict.Add(BBOperaType.MIDDLE, -1);
            self.PressingDict.Add(BBOperaType.RIGHT, -1);
            self.PressingDict.Add(BBOperaType.UPLEFT, -1);
            self.PressingDict.Add(BBOperaType.UP, -1);
            self.PressingDict.Add(BBOperaType.UPRIGHT, -1);
            self.PressingDict.Add(BBOperaType.X, -1);
            self.PressingDict.Add(BBOperaType.A, -1);
            self.PressingDict.Add(BBOperaType.Y, -1);
            self.PressingDict.Add(BBOperaType.B, -1);
            self.PressingDict.Add(BBOperaType.RB, -1);
            self.PressingDict.Add(BBOperaType.RT, -1);
            self.PressingDict.Add(BBOperaType.LB, -1);
            self.PressingDict.Add(BBOperaType.LT, -1);
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

            self.HandlePressingInput(ops, BBOperaType.DOWNLEFT);
            self.HandlePressingInput(ops, BBOperaType.DOWN);
            self.HandlePressingInput(ops, BBOperaType.DOWNRIGHT);
            self.HandlePressingInput(ops, BBOperaType.LEFT);
            self.HandlePressingInput(ops, BBOperaType.MIDDLE);
            self.HandlePressingInput(ops, BBOperaType.RIGHT);
            self.HandlePressingInput(ops, BBOperaType.UPLEFT);
            self.HandlePressingInput(ops, BBOperaType.UP);
            self.HandlePressingInput(ops, BBOperaType.UPRIGHT);
            self.HandlePressingInput(ops, BBOperaType.X);
            self.HandlePressingInput(ops, BBOperaType.A);
            self.HandlePressingInput(ops, BBOperaType.Y);
            self.HandlePressingInput(ops, BBOperaType.B);
            self.HandlePressingInput(ops, BBOperaType.RB);
            self.HandlePressingInput(ops, BBOperaType.RT);
            self.HandlePressingInput(ops, BBOperaType.LB);
            self.HandlePressingInput(ops, BBOperaType.LT);
        }
        
        private static void HandleKeyInput(this InputWait self, long ops, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            bool ret = (ops & operaType) != 0;
            //更新按键是在哪一帧按下的
            if (ret && self.PressingDict[operaType] < sceneTimer.GetNow()) //从松开到按下状态
            {
                self.PressedDict[operaType] = sceneTimer.GetNow();
            }
        }

        private static void HandlePressingInput(this InputWait self, long ops, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            bool ret = (ops & operaType) != 0;
            if (!ret)
            {
                return;
            }
            self.PressingDict[operaType] = sceneTimer.GetNow();
        }
        
        public static bool WasPressedThisFrame(this InputWait self, long operaType)
        {
            return self.PressedDict[operaType] == BBTimerManager.Instance.SceneTimer().GetNow();
        }

        public static bool IsPressed(this InputWait self, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            return self.PressingDict[operaType] == sceneTimer.GetNow();
        }

        public static bool IsReleased(this InputWait self, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            return self.PressingDict[operaType] < sceneTimer.GetNow();
        }

        public static long GetPressedFrame(this InputWait self, int operaType)
        {
            return self.PressedDict[operaType];
        }
        
        /// <summary>
        /// 比如5帧之前按下x键之后松开，设置有效帧数为6帧，则此时仍然可以判定x为按下状态
        /// </summary>
        /// <param name="self"></param>
        /// <param name="operaType">要查询的指令</param>
        /// <param name="buffFrame">在给定帧数内认为按键仍然有效</param>
        /// <returns></returns>
        public static bool IsKeyCached(this InputWait self, int operaType, int buffFrame = 5)
        {
            if (self.PressingDict[operaType] == -1)
            {
                return false;
            }

            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            return self.PressingDict[operaType] + buffFrame >= sceneTimer.GetNow();
        }
        
        #endregion
        
        //https://www.zhihu.com/question/36951135/answer/69880133
        private static void Notify(this InputWait self,long op)
        {
            for (int i = 0; i < self.tcss.Count; i++)
            {
                BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
                InputCallback callback = self.tcss[i];
                
                //1.  检测回调是否过期
                // if (callback.timeOut != -1 && callback.timeOut < sceneTimer.GetNow())
                // {
                //     callback.SetResult(new WaitInput(){frame = 0, Error = WaitTypeError.Timeout});
                //     self.tcss.Remove(callback);
                //     continue;
                // }
                
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
            // if (timeOut != -1)
            // {
            //     timeOut = BBTimerManager.Instance.SceneTimer().GetNow() + timeOut;
            // }
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

        private static async ETTask Test(this InputWait self,string handler)
        {
            Log.Warning(handler);
            await ETTask.CompletedTask;
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
        }
        
        public static bool ContainKey(this InputWait self, long op)
        {
            return (self.curOP & op) != 0;
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