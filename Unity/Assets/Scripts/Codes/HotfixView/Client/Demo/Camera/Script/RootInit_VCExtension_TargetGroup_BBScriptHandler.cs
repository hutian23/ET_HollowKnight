namespace ET.Client
{
    public class RootInit_VCExtension_TargetGroup_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VCExtension_TargetGroup";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent lateUpdateTimer = BBTimerManager.Instance.LateUpdateTimer();

            //1. 初始化
            parser.TryRemoveParam("VC_TargetGroup");
            
            //2. 
            // ListComponent<>
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}