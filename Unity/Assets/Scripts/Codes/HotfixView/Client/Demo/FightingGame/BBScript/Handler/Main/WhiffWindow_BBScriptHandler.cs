namespace ET.Client
{
    [Invoke(BBTimerInvokeType.WhiffWindowTimer)]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BehaviorInfo))]
    public class WhiffWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            int currentOrder = -1;
            foreach (int order in buffer.WhiffOptions)
            {
                BehaviorInfo info = buffer.GetInfoByOrder(order);
                if (info.BehaviorCheck())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }

            if (currentOrder == -1)
            {
                return;
            }
            
            //初始化
            buffer.BehaviorCheckList.Clear();
            bbTimer.Remove(ref buffer.CheckTimer);
            //切换行为
            timelineComponent.Reload(currentOrder);
        }
    }

    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorBuffer))]
    public class WhiffWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WhiffWindow";
        }

        //如果是在动画帧携程中，执行完动画帧事件会自动销毁携程，移除timer
        //因此timer的销毁需要添加到parser.token
        //OpenWhiffWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            //1. 初始化
            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.BehaviorCheckList.Clear();
            buffer.WhiffOptions.Clear();
            
            //2. 定时器
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.WhiffWindowTimer, bbParser);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref buffer.CheckTimer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}