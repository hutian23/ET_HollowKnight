namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(TimelineComponent))]
    [FriendOf(typeof(BehaviorInfo))]
    public class BeforeBehaviorReload_Dummy_ReloadTimeline : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();
            BehaviorInfo info = machine.GetInfoByOrder(args.behaviorOrder);
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            // 显示层，更新PlayableGraph
            bbParser.GetParent<TimelineComponent>().GetTimelinePlayer().Init(info.Timeline);
            // 逻辑层，取消当前所有子协程
            bbParser.Cancel();
            
            // 记录CurrentOrder, 缓存共享变量
            machine.SetCurrentOrder(args.behaviorOrder);
            foreach (var kv in machine.paramDict)
            {
                SharedVariable variable = kv.Value;
                bbParser.RegistParam(variable.name, variable.value);
            }
            machine.ClearParam();

            // 清空行为携程中生成的组件
            foreach (var kv in timelineComponent.markerEventDict)
            {
                TimelineMarkerEvent markerEvent = timelineComponent.GetChild<TimelineMarkerEvent>(kv.Value);
                markerEvent.Dispose();
            }
            timelineComponent.markerEventDict.Clear();

            await ETTask.CompletedTask;
        }
    }
}