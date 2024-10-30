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
            Match match = Regex.Match(data.opLine,"NumericCheck: (?<NumericType>.*?) (?<OP>.*?) (?<CheckValue>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return false;
            }

            long value = parser.GetParent<TimelineComponent>().GetParam<long>(match.Groups["NumericType"].Value);
            if (!long.TryParse(match.Groups["CheckValue"].Value, out long checkValue))
            {
                Log.Error($"cannot convert {match.Groups["CheckValue"].Value} to long");
                return false;
            }
            
            switch (match.Groups["OP"].Value)
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