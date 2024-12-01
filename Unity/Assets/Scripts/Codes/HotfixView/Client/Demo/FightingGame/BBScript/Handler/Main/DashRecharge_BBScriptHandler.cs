namespace ET.Client
{
    public class DashRecharge_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DashRecharge"; 
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Log.Warning("Hello_World");
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}