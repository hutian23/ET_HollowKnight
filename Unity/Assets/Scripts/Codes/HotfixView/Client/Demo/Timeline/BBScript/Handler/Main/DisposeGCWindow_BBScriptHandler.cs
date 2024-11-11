namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    public class DisposeGCWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisposeGCWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.GCOptions.Clear();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}