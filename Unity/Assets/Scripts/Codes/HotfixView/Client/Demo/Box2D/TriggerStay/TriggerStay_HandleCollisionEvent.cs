using ET.Event;
using MongoDB.Bson;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerStayType.CollisionEvent)]
    [FriendOf(typeof(b2Body))]
    public class TriggerStay_HandleCollisionEvent : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            
            //1. find trigger event
            BoxInfo info = args.dataA.UserData as BoxInfo;
            if(!hitboxComponent.ContainTriggerEvent(info.boxName)) return;
            
            TriggerEvent triggerEvent = hitboxComponent.GetTriggerEvent(info.boxName);
            Log.Warning(triggerEvent.ToJson());
        }
    }
}