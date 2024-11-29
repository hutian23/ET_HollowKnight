namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.HitboxComponent))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Log.Warning(parser.GetParam<bool>("Transition_MiddleLand").ToString());
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}