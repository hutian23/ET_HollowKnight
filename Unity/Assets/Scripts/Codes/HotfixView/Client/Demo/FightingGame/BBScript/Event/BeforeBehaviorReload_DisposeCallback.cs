namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(TimelineComponent))]
    public class BeforeBehaviorReload_DisposeCallback : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();

            //清空回调
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

            await ETTask.CompletedTask;
        }
    }
}