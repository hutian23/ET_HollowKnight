using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BehaviorInfo))]
    public class WhiffOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WhiffOption";
        }

        //WhiffOption: 'Mai_Run';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"WhiffOption: '(?<Option>\w+)';");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();
            BehaviorInfo info = machine.GetInfoByName(match.Groups["Option"].Value);
            if (info.behaviorOrder == machine.currentOrder)
            {
                Log.Error($"can not add same behavior into whiffCancel!");
                return Status.Failed;
            }
            
            machine.WhiffOptions.Add(info.behaviorOrder);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}