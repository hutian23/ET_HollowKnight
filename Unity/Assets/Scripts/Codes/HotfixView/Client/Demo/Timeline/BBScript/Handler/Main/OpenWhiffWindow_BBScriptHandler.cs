namespace ET.Client
{
    [Invoke(BBTimerInvokeType.WhiffWindowTimer)]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BehaviorInfo))]
    public class WhiffWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            //预输入
            InputWait inputWait = self.GetParent<TimelineComponent>().GetComponent<InputWait>();
            inputWait.InputNotify();

            BehaviorBuffer buffer = self.GetParent<TimelineComponent>().GetComponent<BehaviorBuffer>();
            foreach (int option in buffer.WhiffOptions)
            {
                BehaviorInfo info = buffer.GetInfoByOrder(option);
                bool ret = info.BehaviorCheck();
                if (ret)
                {
                    self.GetParent<TimelineComponent>().Reload(info.Timeline, info.behaviorOrder);
                    break;
                }
            }
        }
    }

    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorBuffer))]
    public class OpenWhiffWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OpenWhiffWindow";
        }

        //如果是在动画帧携程中，执行完动画帧事件会自动销毁携程，移除timer
        //因此timer的销毁需要添加到parser.token
        //OpenWhiffWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            bbTimer.Remove(ref buffer.CheckTimer);
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.WhiffWindowTimer, parser);
            buffer.CheckTimer = timer;
            parser.cancellationToken.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}