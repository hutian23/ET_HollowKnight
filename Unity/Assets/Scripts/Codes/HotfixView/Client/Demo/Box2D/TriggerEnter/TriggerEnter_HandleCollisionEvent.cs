using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerEnterType.CollisionEvent)]
    [FriendOf(typeof(b2Body))]
    public class TriggerEnter_HandleCollisionEvent : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
        }
    }
}