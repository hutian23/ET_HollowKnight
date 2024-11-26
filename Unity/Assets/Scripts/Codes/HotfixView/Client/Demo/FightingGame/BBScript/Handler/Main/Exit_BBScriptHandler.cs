namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class Exit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Exit";
        }

        //Exit;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            //取消行为
            // EventSystem.Instance.Invoke(new CancelBehaviorCallback() { instanceId = bbParser.InstanceId });
            bbParser.cancellationToken.Cancel();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}