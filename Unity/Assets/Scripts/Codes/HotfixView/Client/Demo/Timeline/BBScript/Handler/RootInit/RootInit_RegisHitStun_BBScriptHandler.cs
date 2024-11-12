using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BehaviorBuffer))]
    public class RootInit_RegisHitStun_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistHitStun";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "RegistHitStun: (?<BehaviorName>.*?), (?<HitFlag>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string behaviorName = match.Groups["BehaviorName"].Value;
            string hitFlag = match.Groups["HitFlag"].Value;

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimeline timeline = timelineComponent.GetTimelinePlayer().GetTimeline(behaviorName);

            BehaviorInfo info = buffer.AddChild<BehaviorInfo>();

            int order = buffer.Children.Count - 1;
            info.Timeline = timeline;
            info.behaviorOrder = order;
            info.behaviorName = timeline.timelineName;
            info.moveType = MoveType.HitStun;
            buffer.behaviorNameMap.Add(info.behaviorName, info.Id);
            buffer.behaviorOrderMap.Add(info.behaviorOrder, info.Id);
            buffer.hitStunFlagMap.Add(hitFlag, info.Id);
            
            info.LoadSkillInfo(timeline);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}