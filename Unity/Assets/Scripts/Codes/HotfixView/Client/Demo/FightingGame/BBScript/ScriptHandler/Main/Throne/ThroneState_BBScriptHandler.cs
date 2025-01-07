using System.Text.RegularExpressions;

namespace ET.Client
{
    public class ThroneState_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ThroneState";
        }

        //ThroneState: 1, Boss_WallThrow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"ThroneState: (?<No>.*?), (?<StateName>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            long instanceId = 0;
            switch (match.Groups["No"].Value)
            {
                case "1":
                    instanceId = timelineComponent.GetParam<long>("Throne_1");
                    break;
                case "2":
                    instanceId = timelineComponent.GetParam<long>("Throne_2");
                    break;
                case "3":
                    instanceId = timelineComponent.GetParam<long>("Throne_3");
                    break;
                default:
                    return Status.Failed;
            }
            TimelineComponent _timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
            _timelineComponent.Reload(match.Groups["StateName"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}