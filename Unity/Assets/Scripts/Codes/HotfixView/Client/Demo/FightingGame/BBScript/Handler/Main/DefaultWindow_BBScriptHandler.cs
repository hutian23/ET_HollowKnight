namespace ET.Client
{
    [Invoke(BBTimerInvokeType.DefaultWindowTimer)]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BehaviorBuffer))]
    public class DefaultWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            int currentOrder = buffer.GetCurrentOrder();
            for (int i = 0; i < buffer.BehaviorCheckList.Count; i++)
            {
                long id = buffer.BehaviorCheckList[i];
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(id);
                if (info.BehaviorCheck())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }

            if (currentOrder == buffer.GetCurrentOrder())
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
    [FriendOf(typeof(BehaviorInfo))]
    public class DefaultWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DefaultWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
         
            //1. 初始化
            bbTimer.Remove(ref buffer.CheckTimer);
            buffer.BehaviorCheckList.Clear();
            
            //2. 获取全部当前可切换的行为
            int currentOrder = buffer.GetCurrentOrder();
            foreach (long id in buffer.DescendInfoList)
            {
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(id);
                //只遍历权值比当前行为高的
                if (info.behaviorOrder == currentOrder)
                {
                    break;
                }
                //一些特殊行为不会添加到行为机中
                if (info.moveType is MoveType.HitStun || info.moveType is MoveType.Etc)
                {
                    continue;
                }
                buffer.BehaviorCheckList.Add(info.Id);
            }
            
            //3. 启动行为机定时器
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.DefaultWindowTimer, bbParser);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref buffer.CheckTimer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}