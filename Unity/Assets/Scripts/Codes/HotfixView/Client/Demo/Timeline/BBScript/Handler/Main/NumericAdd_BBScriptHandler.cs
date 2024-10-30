using System.Text.RegularExpressions;

namespace ET.Client
{
    public class NumericAdd_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "NumericAdd";
        }

        //NumericAdd: DashCount, -1;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "NumericAdd: (?<NumericType>.*?), (?<OP>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["OP"].Value, out long op))
            {
                Log.Error($"cannot format {match.Groups["OP"].Value} to long");
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            long numeric = timelineComponent.GetParam<long>(match.Groups["NumericType"].Value) + op;
            timelineComponent.UpdateParam(match.Groups["NumericType"].Value,numeric);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}