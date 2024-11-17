namespace ET.Client
{
    [Invoke(BBTimerInvokeType.HitStopTestTimer)]
    public class HitStopTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            Log.Warning("HitStop");
        }
    }

    public class HitStop_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStop";
        }

        //HitStop: 8;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = parser.ClientScene().CurrentScene().GetComponent<BBTimerComponent>();
            long timer = bbTimer.NewOnceTimer(bbTimer.GetNow() + 60, BBTimerInvokeType.HitStopTestTimer, parser);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}