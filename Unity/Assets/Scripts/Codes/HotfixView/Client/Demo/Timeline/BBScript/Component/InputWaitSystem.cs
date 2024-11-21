using System;

namespace ET.Client
{
    [FriendOf(typeof (InputWait))]
    public static class InputWaitSystem
    {
        [Invoke(BBTimerInvokeType.BBInputHandleTimer)]
        [FriendOf(typeof (BBInputComponent))]
        [FriendOf(typeof (InputWait))]
        public class BBInputHandleTimer: BBTimer<InputWait>
        {
            protected override void Run(InputWait self)
            {
                self.Ops = BBInputComponent.Instance.CheckInput();
                //1. 更新按键历史
                self.UpdateKeyHistory(self.Ops);
                //2. 添加缓冲
                self.Notify(self.Ops);
                //2. 更新缓冲区
                self.UpdateBuffer();
            }
        }

        public static void Init(this InputWait self)
        {
            BBTimerManager.Instance.SceneTimer().Remove(ref self.timer);
            self.timer = BBTimerManager.Instance.SceneTimer().NewFrameTimer(BBTimerInvokeType.BBInputHandleTimer, self);
            self.Ops = 0;
            
            self.Token?.Cancel();
            self.tcss.ForEach(tcs => tcs.Recycle());
            self.tcss.Clear();
            self.Token = new ETCancellationToken();

            self.bufferDict.Clear();
            int count = self.bufferQueue.Count;
            while (count-- > 0)
            {
                InputBuffer buffer = self.bufferQueue.Dequeue();
                buffer.Recycle();
            }

            self.PressedDict.Clear();
            self.RegistKeyHistory();
        }

        #region KeyHistory

        private static void RegistKeyHistory(this InputWait self)
        {
            self.PressedDict.Add(BBOperaType.LIGHTPUNCH, long.MaxValue);
            self.PressedDict.Add(BBOperaType.LIGHTKICK, long.MaxValue);
            self.PressedDict.Add(BBOperaType.MIDDLEPUNCH, long.MaxValue);
            self.PressedDict.Add(BBOperaType.HEAVYKICK, long.MaxValue);
            self.PressedDict.Add(BBOperaType.HEAVYPUNCH, long.MaxValue);
        }

        private static void UpdateKeyHistory(this InputWait self, long ops)
        {
            self.HandleKeyInput(ops, BBOperaType.LIGHTPUNCH);
            self.HandleKeyInput(ops, BBOperaType.LIGHTKICK);
            self.HandleKeyInput(ops, BBOperaType.MIDDLEPUNCH);
            self.HandleKeyInput(ops, BBOperaType.MIDDLEKICK);
            self.HandleKeyInput(ops, BBOperaType.HEAVYPUNCH);
            self.HandleKeyInput(ops, BBOperaType.HEAVYKICK);
        }

        private static void HandleKeyInput(this InputWait self, long ops, int operaType)
        {
            BBTimerComponent bbTimer = BBTimerManager.Instance.SceneTimer();
            bool ret = (ops & operaType) != 0;
            //更新按键是在哪一帧按下的
            if (ret)
            {
                if (self.PressedDict[operaType] > bbTimer.GetNow())
                {
                    self.PressedDict[operaType] = bbTimer.GetNow();
                }
            }
            else
            {
                self.PressedDict[operaType] = long.MaxValue;
            }
        }

        public static bool WasPressedThisFrame(this InputWait self, int operaType)
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
                BBTimerComponent bbTimer = BBTimerManager.Instance.SceneTimer();
                InputCallback callback = self.tcss[i];
                //1. 当前输入不符合条件
                switch (callback.waitType)
                {
                    case FuzzyInputType.OR:
                        if ((op & callback.OP) == 0) continue;
                        break;
                    case FuzzyInputType.AND:
                        if ((op & callback.OP) != callback.OP) continue;
                        break;
                }

                if (callback.checkFunc != null && !callback.checkFunc.Invoke()) continue;

                //2. 不同技能可能有不同的判定逻辑
                callback.SetResult(new WaitInput() { frame = bbTimer.GetNow(), Error = WaitTypeError.Success, OP = op });
                self.tcss.Remove(callback);
            }
        }

        public static void InputNotify(this InputWait self)
        {
            //输入窗口打开,处理预输入逻辑
            self.Notify(self.Ops);
        }

        public static async ETTask<WaitInput> Wait(this InputWait self, long OP, int waitType, Func<bool> checkFunc = null)
        {
            InputCallback tcs = InputCallback.Create(OP, waitType, checkFunc);
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

        public static async ETTask InputCheckCor(this InputWait self, BBInputHandler handler, ETCancellationToken token)
        {
            while (true)
            {
                //输入检测携程，等待输入
                TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
                Status ret = await handler.Handle(timelineComponent.GetParent<Unit>(), token);
                if (token.IsCancel()) return;
                
                //输入携程判定成功，添加技能缓冲
                if (ret is Status.Success)
                {
                    //考虑到hitStop(TODO增加说明)
                    BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
                    InputBuffer buffer = InputBuffer.Create(handler, bbTimer.GetNow(), bbTimer.GetNow() + 5);
                    self.AddBuffer(buffer);
                }

                await TimerComponent.Instance.WaitFrameAsync(token);
                if (token.IsCancel()) return;
            }
        }

        private static void AddBuffer(this InputWait self, InputBuffer buffer)
        {
            while (self.bufferQueue.Count >= InputWait.MaxStack)
            {
                InputBuffer oldBuffer = self.bufferQueue.Dequeue();
                oldBuffer.Recycle();
            }
            self.bufferQueue.Enqueue(buffer);
        }

        private static void UpdateBuffer(this InputWait self)
        {
            BBTimerComponent bbTimer = self.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            
            int count = self.bufferQueue.Count;
            while (count-- > 0)
            {
                InputBuffer buffer = self.bufferQueue.Dequeue();
                BBInputHandler handler = buffer.handler;

                if (bbTimer.GetNow() > buffer.lastedFrame)
                {
                    if (self.bufferDict.ContainsKey(handler.GetInputType()))
                    {
                        self.bufferDict[handler.GetInputType()] = false;
                    }

                    buffer.Recycle();
                    continue;
                }

                self.bufferQueue.Enqueue(buffer);
                if (!self.bufferDict.ContainsKey(handler.GetInputType()))
                {
                    self.bufferDict.Add(handler.GetInputType(), true);
                }
                else
                {
                    self.bufferDict[handler.GetInputType()] = true;
                }
            }
        }

        public static bool ContainKey(this InputWait self, long op)
        {
            return (self.Ops & op) != 0;
        }
        
        public static bool CheckInput(this InputWait self, string inputType)
        {
            self.bufferDict.TryGetValue(inputType, out bool value);
            return value;
        }
    }
}