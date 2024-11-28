namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(TimelineComponent))]
    public class BeforeBehaviorReload_DummyReloadBehaviorBuffer : AEvent<BeforeBehaviorReload>
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

            //2. 清空回调
            foreach (var kv in timelineComponent.callbackDict)
            {
                TimelineCallback callback = timelineComponent.GetChild<TimelineCallback>(kv.Value);
                callback.Dispose();
            }
            timelineComponent.callbackDict.Clear();

            foreach (var kv in timelineComponent.markerEventDict)
            {
                TimelineMarkerEvent markerEvent = timelineComponent.GetChild<TimelineMarkerEvent>(kv.Value);
                markerEvent.Dispose();
            }
            timelineComponent.markerEventDict.Clear();

            //3. 缓存共享变量
            foreach (var kv in buffer.paramDict)
            {
                SharedVariable variable = kv.Value;
                bbParser.RegistParam(variable.name, variable.value);
            }
            buffer.ClearParam();
            buffer.GCOptions.Clear();
            buffer.WhiffOptions.Clear();
            
            await ETTask.CompletedTask;
        }
    }
}