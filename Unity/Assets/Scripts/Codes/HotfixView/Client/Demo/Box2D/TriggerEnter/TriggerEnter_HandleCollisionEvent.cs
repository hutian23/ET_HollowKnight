using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerEnterType.CollisionEvent)]
    [FriendOf(typeof(b2Body))]
    public class TriggerEnter_HandleCollisionEvent : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();

            //1. find trigger event
            BoxInfo info = args.dataA.UserData as BoxInfo;
            if (!hitboxComponent.ContainTriggerEvent(info.boxName,TriggerType.TriggerEnter)) return;

            TriggerEvent triggerEvent = hitboxComponent.GetTriggerEvent(info.boxName);
            TriggerHelper.HandleTriggerEventAsync(triggerEvent).Coroutine();
        }
    }
}