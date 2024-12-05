using System.Linq;

namespace ET.Client
{
    //https://www.zhihu.com/question/36951135/answer/69880133
    [FriendOf(typeof (InputWait))]
    public static class InputWaitSystem
    {
        public class InputWaitFrameUpdateSystem: FrameUpdateSystem<InputWait>
        {
            protected override void FrameUpdate(InputWait self)
            {
                self.curOP = BBInputComponent.Instance.CheckInput();
                self.UpdateInput(self.curOP);
                //3. 更新按键历史
                self.UpdateKeyHistory(self.curOP);
            }
        }

        public static void Init(this InputWait self)
        {
            self.curOP = 0;
            self.infoQueue.Clear();
            self.infoList.Clear();

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

        public static bool IsPressing(this InputWait self, int operaType)
        {
            return self.PressingDict[operaType] == BBTimerManager.Instance.SceneTimer().GetNow();
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

        private static void UpdateInput(this InputWait self,long ops)
        {
            self.infoQueue.Enqueue(new InputInfo()
                {
                    op = ops,
                    Flip = b2GameManager.Instance.GetBody(self.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId).GetFlip(),
                    frame = BBTimerManager.Instance.SceneTimer().GetNow()
                });
            
            //超出容量部分出列
            int count = self.infoQueue.Count;
            while (count-- > InputWait.MaxStack)
            {
                self.infoQueue.Dequeue();
            }
            self.infoList = self.infoQueue.ToList();
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