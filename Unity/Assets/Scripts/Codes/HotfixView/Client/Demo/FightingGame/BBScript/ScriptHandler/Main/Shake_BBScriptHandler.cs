using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(ShakeComponent))]
    public class Shake_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Shake";
        }

        //Shake: ShakeLength, totalTime;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Shake: (?<ShakeLength>\w+), (?<ShakeCnt>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["ShakeCnt"].Value, out int shakeCnt))
            {
                Log.Error($"cannot format {match.Groups["ShakeCnt"].Value} to long");
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["ShakeLength"].Value, out int ShakeLength))
            {
                Log.Error($"cannot format {match.Groups["ShakeLength"].Value} to long");
                return Status.Failed;
            }

            ShakeComponent shakeComponent = parser.AddComponent<ShakeComponent>();
            shakeComponent.UnitId = parser.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId;
            shakeComponent.ShakeCnt = shakeCnt;
            shakeComponent.TotalShakeCnt = shakeCnt;
            shakeComponent.ShakeLength = ShakeLength;
            
            token.Add(parser.RemoveComponent<ShakeComponent>);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}