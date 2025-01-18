using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    public class StartTimeline_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "StartTimeline";
        }

        //StartTimeline;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BehaviorInfo behaviorInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());

            //1. 更新PlayableGraph
            BBTimeline _timeline = timelineComponent.GetTimelinePlayer().GetTimeline(behaviorInfo.behaviorName);
            timelineComponent.GetTimelinePlayer().Init(_timeline);
            
            //2. 逐帧执行Timeline
            RuntimePlayable playable = timelineComponent.GetTimelinePlayer().RuntimePlayable;
            for (int i = 0; i < playable.ClipMaxFrame(); i++)
            {
                timelineComponent.Evaluate(i);
                await bbTimer.WaitAsync(1, token);
                if (token.IsCancel()) break;
            }

            return token.IsCancel() ? Status.Failed : Status.Success;
        }
    }
}