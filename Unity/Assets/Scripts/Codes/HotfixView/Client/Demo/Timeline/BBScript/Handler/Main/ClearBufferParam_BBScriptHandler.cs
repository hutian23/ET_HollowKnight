namespace ET.Client
{
    public class ClearBufferParam_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ClearBufferParam";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            buffer.ClearParam();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}
