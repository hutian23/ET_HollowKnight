using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorMachine))]
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
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            //string behaviorName ---> BehaviorOrder
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();
            machine.AddGCOption(match.Groups["Option"].Value);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}