namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    public class BeforeBehaviorReload_PlayerReloadBehaviorBuffer : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            //1. 记录CurrentOrder
            bbParser.RegistParam("CurrentOrder", args.behaviorOrder);
            buffer.SetCurrentOrder(args.behaviorOrder);
            
            //2. 缓存共享变量
            foreach (var kv in buffer.paramDict)
            {
                SharedVariable variable = kv.Value;
                bbParser.RegistParam(variable.name, variable.value);
            }
            buffer.ClearParam();
            buffer.GCOptions.Clear();
            buffer.WhiffOptions.Clear();

            //3. 
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            bbTimer.Remove(ref buffer.CheckTimer);
            // 进入一些特殊行为，不会启动行为机
            if (!bbParser.ContainParam("CancelBehaviorTimer"))
            {
                buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.BehaviorCheckTimer, buffer);
            }
            await ETTask.CompletedTask;
        }
    }
}