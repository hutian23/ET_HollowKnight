namespace ET.Client
{
    public class CM_Priority_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CM_Priority";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}