namespace ET.Client
{
    [FriendOf(typeof(BehaviorMachine))]
    public class DisposeWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisposeWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();
            machine.DisposeWindow();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}