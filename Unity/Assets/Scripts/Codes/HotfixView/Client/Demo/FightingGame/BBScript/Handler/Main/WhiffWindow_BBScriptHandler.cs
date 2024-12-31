namespace ET.Client
{
    [Invoke(BBTimerInvokeType.WhiffWindowTimer)]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BehaviorInfo))]
    public class WhiffWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            
            int currentOrder = -1;
            foreach (int order in buffer.WhiffOptions)
            {
                BehaviorInfo info = buffer.GetInfoByOrder(order);
                if (info.Trigger())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }

            if (currentOrder == -1)
            {
                return;
            }
            
            //初始化
            buffer.DisposeWindow();
            
            //切换行为
            timelineComponent.Reload(currentOrder);
        }
    }

    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(InputWait))]
    public class WhiffWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WhiffWindow";
        }

        //如果是在动画帧携程中，执行完动画帧事件会自动销毁携程，移除timer
        //因此timer的销毁需要添加到parser.token
        //OpenWhiffWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBInputHelper.OpenWindow(parser, token, BBTimerInvokeType.WhiffWindowTimer);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}