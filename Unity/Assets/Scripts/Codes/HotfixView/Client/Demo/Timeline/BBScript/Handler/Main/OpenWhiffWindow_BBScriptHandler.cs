namespace ET.Client
{
    [Invoke(BBTimerInvokeType.WhiffWindowTimer)]
    [FriendOf(typeof(BehaviorBuffer))]
    public class WhiffWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            //预输入
            InputWait inputWait = self.GetParent<TimelineComponent>().GetComponent<InputWait>();
            inputWait.InputNotify();
            
            BehaviorBuffer buffer = self.GetParent<TimelineComponent>().GetComponent<BehaviorBuffer>();
            foreach (int option in buffer.WhiffOptions)
            {
                BehaviorInfo info = buffer.GetInfoByOrder(option);
                bool ret = info.BehaviorCheck();
                Log.Warning(ret.ToString());
            }
        }
    }
    [FriendOf(typeof(BBParser))]
    public class OpenWhiffWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OpenWhiffWindow";
        }

        //如果是在动画帧携程中，执行完动画帧事件会自动销毁携程，移除timer
        //因此timer的销毁需要添加到parser.token
        //OpenWhiffWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = parser.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.WhiffWindowTimer, parser);
            parser.cancellationToken.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}