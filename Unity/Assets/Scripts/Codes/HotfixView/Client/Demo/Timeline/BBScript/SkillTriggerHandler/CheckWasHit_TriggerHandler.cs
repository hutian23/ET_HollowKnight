namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    public class CheckWasHit_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "WasHit";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();

            return false;
        }
    }
}