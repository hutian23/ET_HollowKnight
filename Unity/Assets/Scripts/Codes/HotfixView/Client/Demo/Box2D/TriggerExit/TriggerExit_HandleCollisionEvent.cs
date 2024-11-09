using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerExitType.CollisionEvent)]
    public class TriggerExit_HandleCollisionEvent : AInvokeHandler<TriggerExitCallback>
    {
        public override void Handle(TriggerExitCallback a)
        {
        }
    }
}