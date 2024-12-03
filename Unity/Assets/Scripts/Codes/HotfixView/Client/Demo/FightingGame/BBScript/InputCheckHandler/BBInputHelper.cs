namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(InputWait))]
    public static class BBInputHelper
    {
        public static InputBuffer CreateBuffer(this InputWait self, long lastFrame)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            return new InputBuffer() { curFrame = bbTimer.GetNow(), buffFrame = lastFrame, ret = Status.Success };
        }

        public static InputBuffer DefaultBuffer(this InputWait self)
        {
            return self.CreateBuffer(5);
        }

        //共用代码
        public static void OpenWindow(BBParser parser, ETCancellationToken token,int invokeType)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            //1. 初始化
            buffer.DisposeWindow();

            // 输入系统依赖两个模块, 控制器模块和输入模块
            //2. 启动行为机定时器
            buffer.WindowTimer = bbTimer.NewFrameTimer(invokeType, bbParser);
            
            token.Add(() =>
            {
                buffer.DisposeWindow();
            });
        }
    }
}