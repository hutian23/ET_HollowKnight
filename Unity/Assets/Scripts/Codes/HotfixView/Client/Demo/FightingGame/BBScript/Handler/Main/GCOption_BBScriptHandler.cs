using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorBuffer))]
    [FriendOf(typeof (BehaviorInfo))]
    public class GCOption_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GCOption";
        }

        //AddCancelOption: '';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"GCOption: '(?<Option>\w+)';");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            //string behaviorName ---> BehaviorOrder
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            buffer.AddGCOption(match.Groups["Option"].Value);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}