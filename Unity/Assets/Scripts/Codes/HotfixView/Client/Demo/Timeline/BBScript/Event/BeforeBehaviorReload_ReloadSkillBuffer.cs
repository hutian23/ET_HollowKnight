namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    public class BeforeBehaviorReload_ReloadSkillBuffer : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            buffer.SetCurrentOrder(args.behaviorOrder);

            //1. 重新启动行为机定时器
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref buffer.CheckTimer);

            //2. 进入HitStun后，只能被其他受击状态取消
            BehaviorInfo info = buffer.GetInfoByOrder(args.behaviorOrder);
            
            if (info.moveType == MoveType.HitStun) return;
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.BehaviorCheckTimer, buffer); 
            await ETTask.CompletedTask;
        }
    }
}