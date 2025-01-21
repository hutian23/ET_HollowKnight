using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class HitStop_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        //HitStop: 6, 8;(Hertz, hitStopFrame)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitStop: (?<TimeScale>.*?), (?<HitStop>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["TimeScale"].Value, out int hertz))
            {
                Log.Error($"cannot format timeScale to int!");
                return Status.Failed;
            }
            if (!int.TryParse(match.Groups["HitStop"].Value, out int hitStop))
            {
                Log.Error($"cannot format HitStop to int!");
                return Status.Failed;
            }
            if (hitStop == 0)
            {
                return Status.Success;
            }

            HitStopCor(parser, hertz, hitStop).Coroutine();

            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask HitStopCor(BBParser parser, int hertz, int hitStop)
        {
            Unit unit = parser.GetParent<Unit>();
            BBNumeric numeric = unit.GetComponent<BBNumeric>();
            BBTimerComponent sceneTimer = BBTimerManager.Instance.SceneTimer();

            numeric.Set("Hertz", hertz);
            
            await sceneTimer.WaitAsync(hitStop, parser.CancellationToken);
            if (parser.CancellationToken.IsCancel()) return;

            numeric.Set("Hertz", 60);
        }
    }
}