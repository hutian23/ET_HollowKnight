using ET.Event;
namespace ET.Client
{
    [Invoke(TriggerStayType.HitCollision)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BehaviorInfo))]
    public class TriggerStay_HitCollision : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
        }
    }
}