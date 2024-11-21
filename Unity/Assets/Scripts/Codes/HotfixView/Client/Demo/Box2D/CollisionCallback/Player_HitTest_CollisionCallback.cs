using System.Collections.Generic;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(b2Body))]
    public class Player_HitTest_CollisionCallback : CollisionCallback
    {
        public override string GetCollisionType()
        {
            return "HitTest";
        }

        public override void Handle(BBParser parser)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();

            //缓存已经触发过攻击回调的unit
            HashSet<long> hitBuffer = parser.GetParam<HashSet<long>>("HitBuffer");

            int count = hitboxComponent.infoQueue.Count;
            while (count-- > 0)
            {
                CollisionInfo info = hitboxComponent.infoQueue.Dequeue();
                BoxInfo boxInfoA = info.dataA.UserData as BoxInfo;
                BoxInfo boxInfoB = info.dataB.UserData as BoxInfo;

                b2Body b2Body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
                
                //打击框和受击框重合,这里希望打击持续帧内只对同一unit造成一次攻击
                if (boxInfoA.hitboxType is HitboxType.Hit && 
                    boxInfoB.hitboxType is HitboxType.Hurt &&
                    b2Body!=null && !hitBuffer.Contains(b2Body.unitId))
                {
                    Log.Warning("Hit!!!");
                    hitBuffer.Add(b2Body.unitId);
                }

                hitboxComponent.infoQueue.Enqueue(info);
            }
        }

        public override void Regist(BBParser parser)
        {
            parser.RegistParam("HitBuffer", new HashSet<long>());
        }

        public override void Dispose(BBParser parser)
        {
            parser.TryRemoveParam("HitBuffer");
        }
    }
}