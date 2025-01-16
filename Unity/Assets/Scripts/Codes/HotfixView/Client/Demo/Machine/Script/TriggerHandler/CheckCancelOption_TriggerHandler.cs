using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckCancelOption_TriggerHandler: BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "CancelOption";
        }

        //CancelOption: 'DashDrive';
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, "CancelOption: '(?<Option>.*?)';");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();

            if (!machine.ContainParam("CancelWindow_Options"))
            {
                return false;
            }
            HashSetComponent<string> options = machine.GetParam<HashSetComponent<string>>("CancelWindow_Options");
            return options.Contains(match.Groups["Option"].Value);
        }
    }
}