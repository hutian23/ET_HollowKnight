using System.Text.RegularExpressions;

namespace ET.Client
{
    public class CheckNumeric_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "NumericCheck";
        }

        public override bool Check(BBParser parser, BBScriptData data)
        {
            Match match = Regex.Match(data.opLine,@"(\w+)\s*([<> =])\s*(\d+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            long value = timelineComponent.GetParam<long>(match.Groups[1].Value);
            if (!long.TryParse(match.Groups[3].Value, out long checkValue))
            {
                Log.Error($"cannot convert {match.Groups["CheckValue"].Value} to long");
                return false;
            }
            
            switch (match.Groups[2].Value)
            {
                case "<":
                    return value < checkValue;
                case "=":
                    return value == checkValue;
                case ">":
                    return value > checkValue;
                default:
                    return false;
            }
        }
    }
}