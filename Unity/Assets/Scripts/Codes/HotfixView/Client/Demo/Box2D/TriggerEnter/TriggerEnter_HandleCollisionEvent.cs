using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerEnterType.CollisionEvent)]
    [FriendOf(typeof(b2Body))]
    public class TriggerEnter_HandleCollisionEvent : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            CollisionInfo info = args.info;
            
            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
        }
    }
}