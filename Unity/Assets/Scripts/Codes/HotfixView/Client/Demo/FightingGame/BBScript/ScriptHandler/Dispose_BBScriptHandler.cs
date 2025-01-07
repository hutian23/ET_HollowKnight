namespace ET.Client
{
    public class Dispose_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Dispose";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            parser.GetParent<TimelineComponent>().GetParent<Unit>().Dispose();
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}