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
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            int currenOrder = -1;
            foreach (long infoId in buffer.DescendInfoList)
            {
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(infoId);
                if (info.behaviorOrder == 0 || info.moveType is MoveType.Etc || info.moveType is MoveType.HitStun)
                {
                    continue;
                }

                if (info.BehaviorCheck())
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
            buffer.BehaviorCheckList.Clear();
            bbTimer.Remove(ref buffer.CheckTimer);
            //切换行为
            timelineComponent.Reload(currenOrder);
        }
    }

    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    public class TransitionWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TransitionWindow";
        }

        //OpenTransitionWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            
            //1. 初始化
            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.BehaviorCheckList.Clear();
            
            //3. 过渡行为可以被
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.TransitionWindowTimer, bbParser);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref buffer.CheckTimer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}