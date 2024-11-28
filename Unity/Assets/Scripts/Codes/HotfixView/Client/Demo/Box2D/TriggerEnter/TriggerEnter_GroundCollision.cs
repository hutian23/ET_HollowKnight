using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerEnterType.GroundCollision)]
    public class TriggerEnter_GroundCollision : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            CollisionInfo info = args.info;
            if (info.dataB.LayerMask != LayerType.Ground)
            {
                return;
            }
            
            TimelineComponent timelineComponent = Root.Instance.Get(info.dataA.InstanceId) as TimelineComponent;
            timelineComponent.UpdateParam("InAir", false);

            b2Body body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            EventSystem.Instance.Invoke(new OnGroundChanged(){instanceId = timelineComponent.InstanceId, OnGround = true});
        }
    }
}