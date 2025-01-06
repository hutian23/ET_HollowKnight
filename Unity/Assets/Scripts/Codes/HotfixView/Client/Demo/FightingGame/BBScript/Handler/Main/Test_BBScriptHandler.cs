using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2Unit))]
    [FriendOf(typeof(InputWait))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Log.Warning(parser.GetParam<string>("PosX"));
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}