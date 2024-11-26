using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(TimelineMarkerEvent))]
    public class HandleUpdateMarkerEventCallback : AInvokeHandler<UpdateEventTrackCallback>
    {
        public override void Handle(UpdateEventTrackCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null)
            {
                Log.Error($"cannot find timeline component: {args.instanceId}");
                return;
            }
            
            //不存在帧事件
            TimelineMarkerEvent markerEvent = timelineComponent.GetMarkerEvent(args.markerName);
            if (markerEvent == null)
            {
                return;
            }
            
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            bbParser.RegistSubCoroutine(markerEvent.startIndex, markerEvent.endIndex, markerEvent.markerName).Coroutine();
        }
    }
}