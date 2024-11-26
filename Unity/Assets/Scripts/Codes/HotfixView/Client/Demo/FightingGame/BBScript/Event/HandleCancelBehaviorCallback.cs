namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BehaviorInfo))]
    public class HandleCancelBehaviorCallback : AInvokeHandler<CancelBehaviorCallback>
    {
        public override void Handle(CancelBehaviorCallback args)
        {
            //1.取消当前行为协程
            BBParser parser = Root.Instance.Get(args.instanceId) as BBParser;

            //2. 行为机控制器重新遍历可进入行为
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            //3. 遍历所有可执行行为，找到一个进入
            buffer.SetCurrentOrder(-1);
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.BehaviorCheckTimer, buffer);
        }
    }
} 