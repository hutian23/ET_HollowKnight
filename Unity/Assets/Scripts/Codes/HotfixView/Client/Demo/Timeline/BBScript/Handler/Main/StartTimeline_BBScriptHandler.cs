using Timeline;

namespace ET.Client
{
    public class StartTime_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "StartTimeline";
        }

        //StartTimeline;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent timer = timelineComponent.GetComponent<BBTimerComponent>();
            RuntimePlayable playable = timelineComponent.GetTimelinePlayer().RuntimePlayable;

            int count = 0;
            for (int i = 0; i < playable.ClipMaxFrame(); i++)
            {
                timelineComponent.Evaluate(i);
                await timer.WaitAsync(1, token);
                count++;
                if (token.IsCancel()) break;
            }
            Log.Warning(count.ToString());
            
            return token.IsCancel() ? Status.Failed : Status.Success;
        }
    }
}