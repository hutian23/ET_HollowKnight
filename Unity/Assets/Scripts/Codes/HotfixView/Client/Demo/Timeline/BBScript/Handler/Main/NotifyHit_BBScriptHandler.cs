namespace ET.Client
{
    public class NotifyHit_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "NotifyHit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            ObjectWait objectWait = timelineComponent.GetComponent<ObjectWait>();
            
            objectWait.Notify(new WaitHit(){Error = WaitTypeError.Success});
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}