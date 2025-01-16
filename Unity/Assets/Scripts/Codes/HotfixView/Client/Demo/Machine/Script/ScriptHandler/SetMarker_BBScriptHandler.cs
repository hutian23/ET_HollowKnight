namespace ET.Client
{
    public class SetMarker_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetMarker";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}