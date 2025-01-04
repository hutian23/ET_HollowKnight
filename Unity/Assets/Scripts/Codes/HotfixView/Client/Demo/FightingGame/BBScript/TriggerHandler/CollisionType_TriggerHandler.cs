using ET.Event;
using Timeline;

namespace ET.Client
{
    public class CollisionType_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "CollisionType";
        }

        //CollisionType: Unit;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            CollisionInfo info = parser.GetParam<CollisionInfo>("CollisionInfo");
            BoxInfo boxInfo = info.dataB.UserData as BoxInfo;
            return boxInfo.hitboxType is HitboxType.Squash;
        }
    }
}