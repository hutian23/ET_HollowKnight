namespace ET.Client
{
    public class Root_VC_Init_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Init";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            parser.RemoveComponent<VirtualCamera>();
            parser.AddComponent<VirtualCamera>();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}