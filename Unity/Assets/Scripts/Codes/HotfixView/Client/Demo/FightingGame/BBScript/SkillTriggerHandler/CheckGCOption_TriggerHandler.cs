using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckGCOption_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "GCOption";
        }

        //GCOption: 'DashDrive';
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, "GCOption: '(?<GCOption>.*?)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            
            return buffer.ContainGCOption(match.Groups["GCOption"].Value);
        }
    }
}