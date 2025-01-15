namespace ET.Client
{
    [FriendOf(typeof(BBTimerComponent))]
    public class NumericChange_UpdateHertz_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "UpdateHertz";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = parser.GetParent<Unit>().GetComponent<BBTimerComponent>();
            B2Unit b2Unit = parser.GetParent<Unit>().GetComponent<B2Unit>();

            int hertz = (int)parser.GetParam<long>("Numeric_NewValue");
            
            //1. 更新战斗定时器timeScale
            bbTimer.SetHertz(hertz);
            bbTimer.Accumulator = 0;
            
            //2. 速度随timeScale缩放
            b2Unit.SetHertz(hertz);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}