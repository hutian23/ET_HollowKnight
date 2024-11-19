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
            b2Body b2Body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            
            float velY = b2Body.GetVelocity().Y;
            float maxFall = - timelineComponent.GetParam<long>("MaxFall") / 1000f;
            float middleFall = - timelineComponent.GetParam<long>("MiddleFall") / 1000f;
            
            if (velY < maxFall)
            {
                buffer.RegistParam($"Transition_MiddleLand", true);
            }
            else if (velY < middleFall)
            {
                buffer.RegistParam($"Transition_MiddleLand", true);
            }
            else
            {
                buffer.RegistParam($"Transition_LightLand", true);
            }
            
            timelineComponent.UpdateParam("OnGround", true);
            EventSystem.Instance.PublishAsync(timelineComponent.ClientScene(), new OnGroundChanged()
            {
                instanceId = timelineComponent.InstanceId,
                OnGround = true
            }).Coroutine();
        }
    }
}