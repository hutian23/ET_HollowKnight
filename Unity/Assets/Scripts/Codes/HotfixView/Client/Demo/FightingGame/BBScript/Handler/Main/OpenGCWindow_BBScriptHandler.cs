namespace ET.Client
{
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    //加特林取消窗口，招式之间相互取消
    public class OpenGCWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OpenGCWindow";
        }

        //OpenGCWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}