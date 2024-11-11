using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerExitType.GroundCollision)]
    public class TriggerExit_GroundCollision : AInvokeHandler<TriggerExitCallback>
    {
        public override void Handle(TriggerExitCallback args)
        {
            if(args.dataB.LayerMask != LayerType.Ground) return;
            
            TimelineComponent timelineComponent = Root.Instance.Get(args.dataA.InstanceId) as TimelineComponent;
            timelineComponent.UpdateParam("OnGround", false);
            EventSystem.Instance.PublishAsync(timelineComponent.ClientScene(), new OnGroundChanged()
            {
                instanceId = timelineComponent.InstanceId,
                OnGround = false
            }).Coroutine();
        }
    }
}