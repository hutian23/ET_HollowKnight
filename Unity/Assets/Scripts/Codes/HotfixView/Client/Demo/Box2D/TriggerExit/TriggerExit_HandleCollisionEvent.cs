using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerExitType.CollisionEvent)]
    [FriendOf(typeof(b2Body))]
    public class TriggerExit_HandleCollisionEvent : AInvokeHandler<TriggerExitCallback>
    {
        public override void Handle(TriggerExitCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();

            //1. find trigger event
            BoxInfo info = args.dataA.UserData as BoxInfo;
            if (!hitboxComponent.ContainTriggerEvent(info.boxName, TriggerType.TriggerExit)) return;

            TriggerEvent triggerEvent = hitboxComponent.GetTriggerEvent(info.boxName);
            TriggerHelper.HandleTriggerEventAsync(triggerEvent,args).Coroutine();
        }
    }
}