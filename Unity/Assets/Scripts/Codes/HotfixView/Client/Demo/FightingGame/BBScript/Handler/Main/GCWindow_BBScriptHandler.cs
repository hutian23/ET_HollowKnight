namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GCWindowTimer)]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BehaviorInfo))]
    public class GCWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            int currentOrder = -1;
            BehaviorInfo curInfo = buffer.GetInfoByOrder(buffer.GetCurrentOrder());
            foreach (long infoId in buffer.DescendInfoList)
            {
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(infoId);
                if (info.moveType is MoveType.HitStun || info.moveType is MoveType.Etc)
                {
                    continue;
                }

                if ((info.moveType > curInfo.moveType || buffer.ContainGCOption(info.behaviorOrder)) && info.BehaviorCheck())
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
            buffer.BehaviorCheckList.Clear();
            bbTimer.Remove(ref buffer.CheckTimer);
            //切换行为
            timelineComponent.Reload(currentOrder);
        }
    }

    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]    
    //加特林取消窗口，招式之间相互取消
    public class GCWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GCWindow";
        }

        //OpenGCWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();

            //1. 初始化
            buffer.GCOptions.Clear();
            buffer.BehaviorCheckList.Clear();
            bbTimer.Remove(ref buffer.CheckTimer);
            
            //4. 启动定时器
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.GCWindowTimer, bbParser);
            token.Add(() =>
            {
                bbTimer.Remove(ref buffer.CheckTimer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}