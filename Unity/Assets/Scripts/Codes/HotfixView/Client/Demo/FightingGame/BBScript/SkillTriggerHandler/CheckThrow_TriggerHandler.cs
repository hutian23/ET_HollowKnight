using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    public class CheckThrow_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Throw";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine,@"Throw: (?<Throw>\w+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            switch (match.Groups["Throw"].Value)
            {
                case "true":
                    return bbParser.ContainParam("ThrowHit");
                case "false":
                    return !bbParser.ContainParam("ThrowHit");
                default:
                    return false;
            }
        }
    }
}