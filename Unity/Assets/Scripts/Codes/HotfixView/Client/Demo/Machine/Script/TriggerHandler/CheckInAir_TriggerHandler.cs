using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckInAir_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "InAir";
        }

        //InAir: true;
        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine, @"InAir: (?<InAir>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return false;
            }

            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            
            bool ret = false;
            switch (match.Groups["InAir"].Value)
            {
                case "true":
                    ret = machine.GetParam<bool>("InAir");
                    break;
                case "false":
                    ret = !machine.GetParam<bool>("InAir");
                    break;
                default:
                    ScriptHelper.ScripMatchError(data.opLine);
                    throw new Exception();
            }
            
            return ret;
        }
    }
}