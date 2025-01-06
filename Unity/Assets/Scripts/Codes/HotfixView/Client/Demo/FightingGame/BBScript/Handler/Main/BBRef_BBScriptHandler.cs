namespace ET.Client
{
    public class BBRef_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BBRef";
        }

        //BBRef: Timeline;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}