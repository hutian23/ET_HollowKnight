using System.Collections.Generic;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.DefaultWindowTimer)]
    [FriendOf(typeof(BehaviorInfo))]
    public class DefaultWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            int currentOrder = buffer.GetCurrentOrder();
            List<long> behaviorCheckList = self.GetParam<List<long>>("BehaviorCheckList");
            for(int i = 0; i < behaviorCheckList.Count; i++)
            {
                long id = behaviorCheckList[i];
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(id);
                if (info.BehaviorCheck())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }

            if (currentOrder == buffer.GetCurrentOrder()) return;
            
            //初始化
            behaviorCheckList.Clear();
            self.TryRemoveParam("BehaviorCheckList");
            
            long timer = self.GetParam<long>("BehaviorCheckTimer");
            bbTimer.Remove(ref timer);
            self.TryRemoveParam("BehaviorCheckTimer");
            
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
            
            //1. 获取全部当前可切换的行为
            int currentOrder = buffer.GetCurrentOrder();
            List<long> behaviorCheckList = new();
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
                behaviorCheckList.Add(info.Id);
            }
            bbParser.RegistParam("BehaviorCheckList", behaviorCheckList);
            
            //2. 启动行为机定时器
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.DefaultWindowTimer, bbParser);
            bbParser.RegistParam("BehaviorCheckTimer", timer);

            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}