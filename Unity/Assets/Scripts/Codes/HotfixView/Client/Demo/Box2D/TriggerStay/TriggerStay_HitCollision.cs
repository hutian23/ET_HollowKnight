using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerStayType.HitCollision)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BehaviorInfo))]
    public class TriggerStay_HitCollision : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            CollisionInfo info = args.info;
            
            BoxInfo boxInfoB = info.dataB.UserData as BoxInfo;
            if (boxInfoB.hitboxType is HitboxType.Hit)
            {
                // Log.Warning("Hit");
            }
        }
    }
}