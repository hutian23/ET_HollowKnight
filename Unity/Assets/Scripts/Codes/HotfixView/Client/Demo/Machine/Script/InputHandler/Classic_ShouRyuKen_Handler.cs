namespace ET.Client
{
    [FriendOf(typeof(InputWait))]
    public class Classic_ShouRyuKen_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "Classic_ShouRyuKen";
        }

        public override string GetBufferType()
        {
            return "ShouRyuKen";
        }

        //TODO 跳过吧，以后再做
        // 6 2 6 x 每个阶段犹豫期为5, 所以20帧前的指令无效
        public override long Handle(InputWait self)
        {
            long curFrame = BBTimerManager.Instance.SceneTimer().GetNow();
            for (int i = 0; i < self.infoList.Count; i++)
            {
                // InputInfo info = self.infoList[i];
                // if (curFrame - info.frame > 20)
                // {
                //     continue;
                // }
                //
                //1. 第一阶段 right: 6 9 3, left: 7 4 1
                // switch ((FlipState)info.Flip)
                // {
                //     case FlipState.Left:
                //     {
                //         long operaType = info.op & (BBOperaType.UPLEFT | BBOperaType.LEFT | BBOperaType.DOWNLEFT);
                //         break;
                //     }
                //     case FlipState.Right:
                //     {
                //         long operaType = info.op & (BBOperaType.UPRIGHT | BBOperaType.RIGHT | BBOperaType.DOWNRIGHT);
                //         break;
                //     }
                // }
                for (int j = i; j < self.infoList.Count; j++)
                {
                    
                }
            }
            
            return -1;
        }
    }
}