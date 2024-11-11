namespace ET.Client
{
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}