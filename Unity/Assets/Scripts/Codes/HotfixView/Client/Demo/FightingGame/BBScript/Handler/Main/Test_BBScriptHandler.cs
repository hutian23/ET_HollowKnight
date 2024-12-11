namespace ET.Client
{
    [FriendOfAttribute(typeof(ET.Client.b2Unit))]
    [FriendOfAttribute(typeof(ET.Client.InputWait))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Log.Warning(BBTimerManager.Instance.SceneTimer().GetNow().ToString());
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}