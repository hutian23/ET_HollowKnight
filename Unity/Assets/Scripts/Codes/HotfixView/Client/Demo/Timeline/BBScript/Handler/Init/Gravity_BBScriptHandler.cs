namespace ET.Client
{
    public class Gravity_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Gravity";
        }

        //Gravity: 45;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}