namespace ET.Client
{
    public class CancelRemoveX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CancelMoveX";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<InputWait>().GetComponent<BBTimerComponent>();

            long timer = bbParser.GetParam<long>("MoveXTimer");
            bbTimer.Remove(ref timer);
            bbParser.RemoveParam("MoveXTimer");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}