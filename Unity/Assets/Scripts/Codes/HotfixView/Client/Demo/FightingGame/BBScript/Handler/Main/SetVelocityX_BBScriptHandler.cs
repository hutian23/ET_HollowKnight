using System.Text.RegularExpressions;

namespace ET.Client
{
    public class SetVelocityX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetVelocityX";
        }

        //SetVelocityX: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetVelocityX:\s*(-?\d+(\.\d+)?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body b2Body = b2GameManager.Instance.GetBody(unit.InstanceId);

            long.TryParse(match.Groups[1].Value, out long velX);
            b2Body.SetVelocityX(velX / 1000f);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}