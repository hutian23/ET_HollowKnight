namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    public class DisposeWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisposeWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            buffer.DisposeWindow();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}