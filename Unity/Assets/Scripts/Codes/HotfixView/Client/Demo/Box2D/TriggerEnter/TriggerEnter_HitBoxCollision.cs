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
            //
            // b2Body B2Body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            // Unit unit = Root.Instance.Get(B2Body.unitId) as Unit;
        }
    }
}