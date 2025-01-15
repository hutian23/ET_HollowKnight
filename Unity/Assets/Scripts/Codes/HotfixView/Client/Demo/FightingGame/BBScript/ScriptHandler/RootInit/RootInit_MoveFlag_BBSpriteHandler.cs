using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorMachine))]
    public class RootInit_MoveFlag_BBSpriteHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "MoveFlag";
        }

        //HitFlag: Hurt1;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"MoveFlag: (?<HitFlag>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();
            BehaviorInfo info = machine.GetChild<BehaviorInfo>(parser.GetParam<long>("InfoId"));
            machine.behaviorFlagDict.TryAdd(match.Groups["MoveFlag"].Value, info.Id);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}