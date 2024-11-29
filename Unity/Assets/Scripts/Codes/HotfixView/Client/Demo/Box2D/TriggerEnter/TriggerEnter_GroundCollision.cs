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
            
            //jump recharge
            int jump = (int)timelineComponent.GetParam<long>("MaxJump");
            timelineComponent.UpdateParam("JumpCount", jump);
            
            
            //Land
            b2Body body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            float MaxFall = -timelineComponent.GetParam<long>("MaxFall") / 1000f;
            if (body.GetVelocity().Y <= MaxFall)
            {
                bbParser.RegistParam("Transition_MiddleLand", true);   
            }
        }
    }
}