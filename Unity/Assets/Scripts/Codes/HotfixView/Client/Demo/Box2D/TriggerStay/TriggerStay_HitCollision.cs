using ET.Event;
using Timeline;
namespace ET.Client
{
    [Invoke(TriggerStayType.HitCollision)]
    [FriendOf(typeof(b2Body))]
    public class TriggerStay_HitCollision : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            BoxInfo boxInfo = args.dataB.UserData as BoxInfo;
            if (boxInfo.hitboxType is not HitboxType.Squash) return;

            b2Body B2Body = Root.Instance.Get(args.dataB.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(B2Body.unitId) as Unit;
            Log.Warning("HitCollision");
        }
    }
}