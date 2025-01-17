﻿using System.Linq;

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
                self.UpdateKeyHistory(self.curOP);
                self.UpdateBuffer();
            }
        }

        public class InputWaitAwakeSystem : AwakeSystem<InputWait>
        {
            protected override void Awake(InputWait self)
            {
                self.Init();
            }
        }

        private static void Init(this InputWait self)
        {
            self.curOP = 0;
            self.infoQueue.Clear();
            self.infoList.Clear();
            self.handleQueue.Clear();

            self.BufferFlag = false;
            self.BufferDict.Clear();
            self.PressedDict.Clear();
            self.IsPressingDict.Clear();
            self.PressingDict.Clear();
            self.RegistKeyHistory();
        }

        #region KeyHistory

        private static void RegistKeyHistory(this InputWait self)
        {
            self.PressedDict.Add(BBOperaType.X, -1);
            self.PressingDict.Add(BBOperaType.X, -1);
            self.IsPressingDict.Add(BBOperaType.X, false);
            
            self.PressedDict.Add(BBOperaType.A, -1);
            self.PressingDict.Add(BBOperaType.A, -1);
            self.IsPressingDict.Add(BBOperaType.A, false);

            self.PressedDict.Add(BBOperaType.Y, -1);
            self.PressingDict.Add(BBOperaType.Y, -1);
            self.IsPressingDict.Add(BBOperaType.Y, false);
            
            self.PressedDict.Add(BBOperaType.B, -1);
            self.PressingDict.Add(BBOperaType.B, -1);
            self.IsPressingDict.Add(BBOperaType.B, false);
            
            self.PressedDict.Add(BBOperaType.RT, -1);
            self.PressingDict.Add(BBOperaType.RT, -1);
            self.IsPressingDict.Add(BBOperaType.RT, false);
            
            self.PressedDict.Add(BBOperaType.RB, -1);
            self.PressingDict.Add(BBOperaType.RB, -1);
            self.IsPressingDict.Add(BBOperaType.RB, false);
            
            self.PressedDict.Add(BBOperaType.LB, -1);
            self.PressingDict.Add(BBOperaType.LB, -1);
            self.IsPressingDict.Add(BBOperaType.LB, false);
            
            self.PressedDict.Add(BBOperaType.LT, -1);
            self.PressingDict.Add(BBOperaType.LT, -1);
            self.IsPressingDict.Add(BBOperaType.LT, false);

            self.PressedDict.Add(BBOperaType.DOWNLEFT, -1);
            self.PressingDict.Add(BBOperaType.DOWNLEFT, -1);
            self.IsPressingDict.Add(BBOperaType.DOWNLEFT, false);

            self.PressedDict.Add(BBOperaType.LEFT, -1);
            self.PressingDict.Add(BBOperaType.LEFT, -1);
            self.IsPressingDict.Add(BBOperaType.LEFT, false);
            
            self.PressedDict.Add(BBOperaType.UPLEFT, -1);
            self.PressingDict.Add(BBOperaType.UPLEFT, -1);
            self.IsPressingDict.Add(BBOperaType.UPLEFT, false);
            
            self.PressedDict.Add(BBOperaType.UP, -1);
            self.PressingDict.Add(BBOperaType.UP, -1);
            self.IsPressingDict.Add(BBOperaType.UP, false);
            
            self.PressedDict.Add(BBOperaType.UPRIGHT, -1);
            self.PressingDict.Add(BBOperaType.UPRIGHT, -1);
            self.IsPressingDict.Add(BBOperaType.UPRIGHT, false);
            
            self.PressedDict.Add(BBOperaType.RIGHT, -1);
            self.PressingDict.Add(BBOperaType.RIGHT, -1);
            self.IsPressingDict.Add(BBOperaType.RIGHT, false);
            
            self.PressedDict.Add(BBOperaType.DOWNRIGHT, -1);
            self.PressingDict.Add(BBOperaType.DOWNRIGHT, -1);
            self.IsPressingDict.Add(BBOperaType.DOWNRIGHT, false);
            
            self.PressedDict.Add(BBOperaType.DOWN, -1);
            self.PressingDict.Add(BBOperaType.DOWN, -1);
            self.IsPressingDict.Add(BBOperaType.DOWN, false);
            
            self.PressedDict.Add(BBOperaType.MIDDLE, -1);
            self.PressingDict.Add(BBOperaType.MIDDLE, -1);
            self.IsPressingDict.Add(BBOperaType.MIDDLE, false);
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
            self.HandleKeyInput(ops, BBOperaType.DOWNLEFT);
            self.HandleKeyInput(ops, BBOperaType.LEFT);
            self.HandleKeyInput(ops, BBOperaType.UPLEFT);
            self.HandleKeyInput(ops, BBOperaType.UP);
            self.HandleKeyInput(ops, BBOperaType.UPRIGHT);
            self.HandleKeyInput(ops, BBOperaType.RIGHT);
            self.HandleKeyInput(ops, BBOperaType.DOWNRIGHT);
            self.HandleKeyInput(ops, BBOperaType.DOWN);
            self.HandleKeyInput(ops, BBOperaType.MIDDLE);
        }
        
        private static void HandleKeyInput(this InputWait self, long ops, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            bool ret = (ops & operaType) != 0;
            if (ret)
            {
                //按键持续按住
                self.PressingDict[operaType] = sceneTimer.GetNow();
                //记录按键哪一帧按下
                if (!self.IsPressingDict[operaType])
                {
                    self.PressedDict[operaType] = sceneTimer.GetNow();
                }
                //按键处于按住状态
                self.IsPressingDict[operaType] = true;
            }
            else
            {
                self.IsPressingDict[operaType] = false;
            }
        }
        
        public static bool WasPressedThisFrame(this InputWait self, long operaType)
        {
            return self.PressedDict[operaType] == BBTimerManager.Instance.SceneTimer().GetNow();
        }

        public static bool WasReleasedThisFrame(this InputWait self, long operaType)
        {
            return self.PressingDict[operaType] >= 0 && BBTimerManager.Instance.SceneTimer().GetNow() - self.PressingDict[operaType] == 1;
        }
        
        public static bool IsPressed(this InputWait self, int operaType)
        {
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            return self.PressingDict[operaType] == sceneTimer.GetNow();
        }

        public static bool IsPressing(this InputWait self, int operaType)
        {
            return self.IsPressingDict[operaType];
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
            self.infoQueue.Enqueue(new InputInfo() { op = ops, frame = BBTimerManager.Instance.SceneTimer().GetNow() });
            
            //超出容量部分出列
            int count = self.infoQueue.Count;
            while (count-- > InputWait.MaxStack)
            {
                self.infoQueue.Dequeue();
            }
            self.infoList = self.infoQueue.ToList();
        }

        private static void UpdateBuffer(this InputWait self)
        {
            //输入缓冲区并未打开
            if (!self.BufferFlag) return;
            
            //RootInit中将InputHandler添加到队列中
            int count = self.handleQueue.Count;
            while (count -- > 0)
            {
                string handlerName = self.handleQueue.Dequeue();
                self.handleQueue.Enqueue(handlerName);
             
                //找到对应handler
                InputHandler handler = ScriptDispatcherComponent.Instance.GetInputHandler(handlerName);
                string bufferType = handler.GetBufferType(); //缓冲类型
                
                //更新缓冲最大有效帧
                long buffFrame = handler.Handle(self);
                if (!self.BufferDict.ContainsKey(bufferType))
                {
                    self.BufferDict.TryAdd(bufferType, -1);
                }
                if (buffFrame > self.BufferDict[bufferType])
                {
                    self.BufferDict[bufferType] = buffFrame;
                }
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