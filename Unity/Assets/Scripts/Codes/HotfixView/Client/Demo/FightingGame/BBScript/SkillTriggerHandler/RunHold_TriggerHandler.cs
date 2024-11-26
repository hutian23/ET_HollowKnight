using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RunHold_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "RunHold";
        }

        //RunHold: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"RunHold: (?<Hold>\w+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            InputWait inputWait = parser.GetParent<TimelineComponent>().GetComponent<InputWait>();

            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();
            Log.Warning(sceneTimer.GetNow() + "   " );
            
            bool ret = inputWait.ContainKey(BBOperaType.RIGHT) || inputWait.ContainKey(BBOperaType.LEFT);

            switch (match.Groups["Hold"].Value)
            {
                case "true":
                    return ret;
                case "false":
                    return !ret;
                default:
                    return false;
            }
        }
    }
}