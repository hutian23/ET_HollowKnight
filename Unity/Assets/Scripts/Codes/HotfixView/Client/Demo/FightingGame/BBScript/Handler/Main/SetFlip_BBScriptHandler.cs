using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    public class SetFlip_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetFlip";
        }

        // SetFlip: Left;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"SetFlip: (?<Flip>\w+);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            Unit unit = timelineComponent.GetParent<Unit>();
            b2Body body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            body.SetFlip(match.Groups["Flip"].Value.Equals("Left")? FlipState.Left : FlipState.Right, true);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}