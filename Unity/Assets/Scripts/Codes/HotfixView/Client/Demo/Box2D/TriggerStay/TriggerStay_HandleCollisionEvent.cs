using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerStayType.CollisionEvent)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(TriggerEvent))]
    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BBCallback))]
    public class TriggerStay_HandleCollisionEvent : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
        }
    }
}