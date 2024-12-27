using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class BallV_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BallV";
        }

        // BallV: 1000, 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BallV: (?<VelX>.*?), (?<VelY>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["VelX"].Value, out long velX) || !long.TryParse(match.Groups["VelY"].Value, out long velY))
            {
                Log.Error($"cannot format {match.Groups["VelX"].Value} / {match.Groups["VelY"].Value} to long!!!");
                return Status.Failed;
            }

            parser.TryRemoveParam("BallStartV");
            parser.RegistParam("BallStartV", new Vector2(velX, velY) / 1000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}