namespace ET.Client
{
    public class Thrones_Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Thrones_Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}