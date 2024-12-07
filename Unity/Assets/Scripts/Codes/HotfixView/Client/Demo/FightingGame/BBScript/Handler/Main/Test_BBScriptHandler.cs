namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.HitboxComponent))]
    [FriendOfAttribute(typeof(ET.Client.InputWait))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Log.Warning(data.CoroutineID.ToString());

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}