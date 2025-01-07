namespace ET.Client
{
    [Invoke(BBTimerInvokeType.TransitionWindowTimer)]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BehaviorInfo))]
    public class TransitionCancelWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            //在过度动画取消窗口内，行为机逻辑上已经进入中立状态，可以切换进所有行为
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            int currenOrder = -1;
            foreach (long infoId in buffer.DescendInfoList)
            {
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(infoId);
                if (info.behaviorOrder == 0 || info.moveType is MoveType.Etc || info.moveType is MoveType.HitStun)
                {
                    continue;
                }

                if (info.Trigger())
                {
                    currenOrder = info.behaviorOrder;
                    break;
                }
            }

            if (currenOrder == -1)
            {
                return;
            }
            
            //初始化
            buffer.DisposeWindow();
            
            //切换行为
            timelineComponent.Reload(currenOrder);
        }
    }

    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(InputWait))]
    public class TransitionWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TransitionWindow";
        }

        //OpenTransitionWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBInputHelper.OpenWindow(parser, token, BBTimerInvokeType.TransitionWindowTimer);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}