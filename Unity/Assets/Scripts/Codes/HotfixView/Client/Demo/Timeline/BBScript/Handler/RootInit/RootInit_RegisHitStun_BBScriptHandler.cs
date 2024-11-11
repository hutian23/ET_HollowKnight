namespace ET.Client
{
    public class RootInit_RegisHitStun_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistHitStun";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}