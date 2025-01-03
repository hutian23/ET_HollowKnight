using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BehaviorBuffer))]
    public class GotoBehavior_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GotoBehavior";
        }

        //GotoBehavior: 'Mai_LandBounce';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"GotoBehavior: '(?<behavior>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            string behavior = match.Groups["behavior"].Value;
            BehaviorInfo info = buffer.GetInfoByName(behavior);

            timelineComponent.Reload(info.behaviorOrder);

            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}