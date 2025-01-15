namespace ET.Client
{
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(InputWait))]
    public static class BBInputHelper
    {
        public static long GetBuffFrame(this InputWait self, int buffFrame)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            return bbTimer.GetNow() + buffFrame;
        }

        //共用代码
        public static void OpenWindow(BBParser parser, ETCancellationToken token,int invokeType)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            //1. 初始化
            machine.DisposeWindow();

            // 输入系统依赖两个模块, 控制器模块和输入模块
            //2. 启动行为机定时器
            machine.WindowTimer = bbTimer.NewFrameTimer(invokeType, parser);
            
            token.Add(() =>
            {
                machine.DisposeWindow();
            });
        }
    }
}