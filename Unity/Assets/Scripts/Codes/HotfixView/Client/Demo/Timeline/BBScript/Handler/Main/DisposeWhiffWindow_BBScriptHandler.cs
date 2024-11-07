namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    public class DisposeWhiffWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisposeWhiffWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.WhiffOptions.Clear();

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}