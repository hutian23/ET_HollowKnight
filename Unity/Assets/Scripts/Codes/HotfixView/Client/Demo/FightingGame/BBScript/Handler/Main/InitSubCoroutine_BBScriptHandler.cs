namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    public class InitSubCoroutine_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "InitSubCoroutine";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}