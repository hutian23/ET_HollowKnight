namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GCWindowTimer)]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BehaviorInfo))]
    public class GCWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();

            int currentOrder = -1;
            BehaviorInfo curInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());
            foreach (long infoId in machine.DescendInfoList)
            {
                BehaviorInfo info = machine.GetChild<BehaviorInfo>(infoId);
                if (info.moveType is MoveType.HitStun || info.moveType is MoveType.Etc)
                {
                    continue;
                }
                
                if ((info.moveType > curInfo.moveType || machine.ContainGCOption(info.behaviorOrder)) && info.Trigger())
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
            machine.DisposeWindow();
            
            //切换行为
            timelineComponent.Reload(currentOrder);
        }
    }

    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(InputWait))]    //加特林取消窗口，招式之间相互取消
    public class GCWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GCWindow";
        }

        //OpenGCWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBInputHelper.OpenWindow(parser, token, BBTimerInvokeType.GCWindowTimer);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}