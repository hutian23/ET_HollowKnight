namespace ET.Client
{
    [Invoke(BBTimerInvokeType.DefaultWindowTimer)]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BehaviorMachine))]
    public class DefaultWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();

            //找到可以切换的行为
            int currentOrder = machine.GetCurrentOrder();
            foreach (long infoId in machine.DescendInfoList)
            {
                BehaviorInfo info = machine.GetChild<BehaviorInfo>(infoId);
                if (info.moveType is MoveType.HitStun || info.moveType is MoveType.Etc)
                {
                    continue;
                }
                if (info.behaviorOrder == currentOrder)
                {
                    break;
                }
                if (info.Trigger())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }
            if (currentOrder == machine.GetCurrentOrder())
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
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(InputWait))]
    public class DefaultWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DefaultWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBInputHelper.OpenWindow(parser, token,BBTimerInvokeType.DefaultWindowTimer);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}