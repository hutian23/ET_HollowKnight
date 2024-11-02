namespace ET.Client
{
    public class LandInertia_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "LandInertia";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}