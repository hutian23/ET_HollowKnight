namespace ET.Client
{
    public class RemoveAirMoveX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RemoveAirMoveX";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            long timer = parser.GetParam<long>("AirMoveXTimer");
            bbTimer.Remove(ref timer);
            
            parser.TryRemoveParam("AirMoveXTimer");
            parser.TryRemoveParam("AirMoveX");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}