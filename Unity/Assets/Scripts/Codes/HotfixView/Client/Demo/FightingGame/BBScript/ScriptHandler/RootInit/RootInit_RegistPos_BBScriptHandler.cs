using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class RootInit_RegistPos_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistPos";
        }

        //RegistPos: -1200, 3900;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"RegistPos: (?<Position>.*?), (?<VelX>-?\d+), (?<VelY>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            if (!long.TryParse(match.Groups["VelX"].Value, out long x) || !long.TryParse(match.Groups["VelY"].Value, out long y))
            {
                Log.Error($"cannot parse {match.Groups["VelX"].Value} / {match.Groups["VelY"].Value} to long!  ");
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            timelineComponent.TryRemoveParam($"Position_{match.Groups["Position"].Value}");
            timelineComponent.RegistParam($"Position_{match.Groups["Position"].Value}", new Vector2(x, y) / 1000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}