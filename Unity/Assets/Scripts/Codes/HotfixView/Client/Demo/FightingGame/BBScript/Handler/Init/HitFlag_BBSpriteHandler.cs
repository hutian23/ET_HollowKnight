using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    public class HitFlag_BBSpriteHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitFlag";
        }

        //HitFlag: Hurt1;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitFlag: (?<HitFlag>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetChild<BehaviorInfo>(parser.GetParam<long>("InfoId"));
            buffer.hitMap.TryAdd(match.Groups["HitFlag"].Value, info.Id);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}