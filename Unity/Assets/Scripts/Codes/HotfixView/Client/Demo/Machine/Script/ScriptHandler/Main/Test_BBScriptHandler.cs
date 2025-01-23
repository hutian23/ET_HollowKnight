using MongoDB.Bson;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(InputWait))]
    [FriendOfAttribute(typeof(ET.Client.BBParser))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            parser.RegistParam("Test_1", 1);
            foreach (var kv in parser.ParamDict)
            {
                Log.Warning(kv.Value.ToJson());
            }
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}