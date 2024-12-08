using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckInputType_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "InputType";
        }

        //InputType: (RunHold);
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"InputType: ((?<InputType>\w+))");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            InputWait wait = timelineComponent.GetComponent<InputWait>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            return wait.CheckBuffer(match.Groups["InputType"].Value,bbTimer.GetNow());
        }
    }
}