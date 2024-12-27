using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class BallLocalPos_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BallLocalPos";
        }

        //BallLocalPos: 1000, 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"BallLocalPos: (?<PosX>.*?), (?<PosY>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["PosX"].Value, out long posX) || !long.TryParse(match.Groups["PosY"].Value, out long posY))
            {
                Log.Error($"cannot format string to long!!!");
                return Status.Failed;
            }
            
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            Vector2 ballPos = body.GetPosition() + new Vector2(posX, posY) / 1000f;
            parser.TryRemoveParam("BallPos");
            parser.RegistParam("BallPos", ballPos);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}