using System.Text.RegularExpressions;

namespace ET.Client
{
    public class SetVelocityY_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetVelocityY";
        }

        //SetVelocityY: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetVelocityY: (?<Velocity>\w+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["Velocity"].Value, out long velocity))
            {
                Log.Error($"cannot format {match.Groups["Velocity"].Value} to long");
                return Status.Failed;
            }
            
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            b2Body body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            body.SetVelocityY(velocity/1000f);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}