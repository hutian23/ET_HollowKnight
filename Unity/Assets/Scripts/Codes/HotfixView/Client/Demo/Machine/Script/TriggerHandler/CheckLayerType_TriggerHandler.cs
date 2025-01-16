namespace ET.Client
{
    public class CheckLayerType_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "LayerType";
        }

        //LayerType: Unit;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            // CollisionInfo info = parser.GetParam<CollisionInfo>("CollisionInfo");
            // BoxInfo boxInfo = info.dataB.UserData as BoxInfo;
            // return boxInfo.hitboxType is HitboxType.Squash;
            //TODO
            return false;
        }
    }
}