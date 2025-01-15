namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GCWindowTimer)]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BehaviorInfo))]
    public class GCWindowTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            //1. 找到能够取消的行为
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
            
            int currentOrder = -1;
            BehaviorInfo curInfo = machine.GetInfoByOrder(machine.GetCurrentOrder());
            HashSetComponent<string> options = machine.GetParam<HashSetComponent<string>>("CancelWindow_Options");
            
            foreach (long infoId in machine.DescendInfoList)
            {
                BehaviorInfo info = machine.GetChild<BehaviorInfo>(infoId);
                if (info.moveType is MoveType.HitStun || info.moveType is MoveType.Etc)
                {
                    continue;
                }
                
                if ((info.moveType > curInfo.moveType || options.Contains(info.behaviorName)) && info.Trigger())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }
            if (currentOrder == -1)
            {
                return;
            }

            //2. 关闭取消窗口
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();
            
            long timer = machine.GetParam<long>("CancelWindow_Timer");
            bbTimer.Remove(ref timer);
            machine.TryRemoveParam("CancelWindow_Timer");
            
            options.Dispose();
            machine.TryRemoveParam("CancelWindow_Options");
            
            //3. 进入行为
            machine.Reload(currentOrder);
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
            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            // 注册变量
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.GCWindowTimer, unit);
            HashSetComponent<string> cancelOptions = HashSetComponent<string>.Create();
            machine.RegistParam("CancelWindow_Timer", timer);
            machine.RegistParam("CancelWindow_Option", cancelOptions);
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer);
                cancelOptions?.Dispose();
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}