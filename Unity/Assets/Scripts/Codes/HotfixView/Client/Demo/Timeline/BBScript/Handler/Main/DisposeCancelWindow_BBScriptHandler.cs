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
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            long timer = parser.GetParam<long>("CancelWindowTimer");
            bbTimer.Remove(ref timer);
            parser.TryRemoveParam("CancelWindowTimer");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}