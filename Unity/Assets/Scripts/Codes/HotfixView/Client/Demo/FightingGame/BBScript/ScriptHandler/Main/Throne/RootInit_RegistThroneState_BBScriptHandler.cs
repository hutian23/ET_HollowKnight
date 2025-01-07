namespace ET.Client
{
    public class RootInit_RegistThroneState_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistThronesState";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}