using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class BallPos_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BallPos";
        }

        //BallPos: 1000, 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "BallPos: (?<posX>.*?), (?<posY>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            if (!long.TryParse(match.Groups["posX"].Value, out long posX) || !long.TryParse(match.Groups["posY"].Value, out long posY))
            {
                Log.Error($"cannot format posX / posY to long");
                return Status.Failed;
            }
            
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            Vector2 curPos = b2Body.GetPosition();
            Vector2 offset = new Vector2(posX, posY) / 1000f;

            long instanceId = parser.GetParam<long>("BallId");
            Unit ball = Root.Instance.Get(instanceId) as Unit;
            b2Body ballBody = b2WorldManager.Instance.GetBody(ball.InstanceId);
            ballBody.SetPosition(curPos + offset);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}