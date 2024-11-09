using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerStayType.CollisionEvent)]
    public class TriggerStay_HandleCollisionEvent : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
        }
    }
}