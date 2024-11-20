using ET.Event;
using Timeline;

namespace ET.Client
{
    public class Player_HitTest_CollisionCallback : CollisionCallback
    {
        public override string GetCollisionType()
        {
            return "HitTest";
        }

        public override bool Handle(BBParser parser, CollisionInfo info)
        {
            BoxInfo infoA = info.dataA.UserData as BoxInfo;
            BoxInfo infoB = info.dataB.UserData as BoxInfo;

            if (infoA.hitboxType is HitboxType.Hit && infoB.hitboxType is HitboxType.Hurt)
            {
                Log.Warning($"Hit!!! {infoA.boxName} --- {infoB.boxName}");
                return true;
            }

            return false;
        }
    }
}