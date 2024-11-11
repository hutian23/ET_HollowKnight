namespace ET.Client
{
    public class DisposeCancelWindow_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisposeCancelWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            long timer = bbParser.GetParam<long>("CancelWindowTimer");
            bbTimer.Remove(ref timer);
            bbParser.TryRemoveParam("CancelWindowTimer");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}