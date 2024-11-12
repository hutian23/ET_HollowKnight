using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerEnterType.HitBoxCollision)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BehaviorInfo))]
    public class TriggerEnter_HitBoxCollision : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            BoxInfo boxInfo = args.dataB.UserData as BoxInfo;
            if (boxInfo.hitboxType is not HitboxType.Squash) return;
            
            b2Body B2Body = Root.Instance.Get(args.dataB.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(B2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetHitStun("GroundHitStun");
        }
    }
}