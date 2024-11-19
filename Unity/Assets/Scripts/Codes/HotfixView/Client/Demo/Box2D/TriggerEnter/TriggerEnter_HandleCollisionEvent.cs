using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerEnterType.CollisionEvent)]
    [FriendOf(typeof(b2Body))]
    [FriendOfAttribute(typeof(ET.Client.b2GameManager))]
    public class TriggerEnter_HandleCollisionEvent : AInvokeHandler<TriggerEnterCallback>
    {
        public override void Handle(TriggerEnterCallback args)
        {
            CollisionInfo info = args.info;

            b2Body b2Body = Root.Instance.Get(info.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();

            BoxInfo info1 = info.dataA.UserData as BoxInfo;
            BoxInfo info2 = info.dataB.UserData as BoxInfo;
            if (info1.hitboxType is HitboxType.Hit && info2.hitboxType is HitboxType.Hurt)
            {
                Log.Warning("Hit Trigger!!!!");
            }
        }
    }
}