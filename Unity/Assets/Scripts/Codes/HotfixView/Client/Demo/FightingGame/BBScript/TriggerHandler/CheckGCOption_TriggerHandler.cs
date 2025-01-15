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
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();
            
            return machine.ContainGCOption(match.Groups["GCOption"].Value);
        }
    }
}