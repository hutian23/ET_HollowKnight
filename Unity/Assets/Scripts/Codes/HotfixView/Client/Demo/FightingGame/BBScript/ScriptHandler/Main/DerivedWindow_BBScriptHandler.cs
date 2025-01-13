namespace ET.Client
{
    //派生取消窗口可以和其他窗口并行执行
    //
    public class DerivedWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DerivedWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}