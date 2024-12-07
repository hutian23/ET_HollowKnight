using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RootInit_NumericType_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "NumericType";
        }

        //NumericType: MaxFall, 300;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "NumericType: (?<NumericType>.*?), (?<Value>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            if (timelineComponent.ContainParam(match.Groups["NumericType"].Value))
            {
                Log.Error($"already exist NumericType:{match.Groups["NumericType"].Value}");
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["Value"].Value, out long value))
            {
                Log.Error($"cannot format {match.Groups["Value"].Value} to long");
            }
            
            timelineComponent.RegistParam(match.Groups["NumericType"].Value, value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}