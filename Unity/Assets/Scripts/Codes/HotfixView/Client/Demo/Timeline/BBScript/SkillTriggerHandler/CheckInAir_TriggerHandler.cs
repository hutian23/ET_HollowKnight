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

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
           
            bool ret = false;
            switch (match.Groups["InAir"].Value)
            {
                case "true":
                    ret = !timelineComponent.GetParam<bool>("OnGround");
                    break;
                case "false":
                    ret = timelineComponent.GetParam<bool>("OnGround");
                    break;
                default:
                    DialogueHelper.ScripMatchError(data.opLine);
                    throw new Exception();
            }
            return ret;
        }
    }
}