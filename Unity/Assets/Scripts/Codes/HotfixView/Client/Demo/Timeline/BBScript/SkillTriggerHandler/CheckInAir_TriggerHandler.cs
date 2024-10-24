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
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }
            
            SkillBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            switch (match.Groups["InAir"].Value)
            {
                case "true":
                    return !buffer.GetParam<bool>("OnGround");
                case "false":
                    return buffer.GetParam<bool>("OnGround");
                default:
                    DialogueHelper.ScripMatchError(data.opLine);
                    throw new Exception();
            }
        }
    }
}