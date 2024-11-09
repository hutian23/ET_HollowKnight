using ET.Event;

namespace ET.Client
{
    [Invoke(TriggerEnterType.HitBoxCollision)]
    public class TriggerEnter_HitBoxCollision: AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback a)
        {
        }
    }
}