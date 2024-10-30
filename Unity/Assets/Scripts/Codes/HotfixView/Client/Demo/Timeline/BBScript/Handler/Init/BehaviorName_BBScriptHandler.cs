using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    public class BehaviorName_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Name";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Name: '(?<BehaviorName>\w+)'");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            BehaviorBuffer buffer = parser.GetParent<TimelineComponent>().GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetChild<BehaviorInfo>(parser.GetParam<long>("InfoId"));
            info.behaviorName = match.Groups["BehaviorName"].Value;

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}