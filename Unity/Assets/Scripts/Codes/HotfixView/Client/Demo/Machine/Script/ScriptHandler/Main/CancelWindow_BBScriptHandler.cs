using System.Text.RegularExpressions;

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

    [Invoke(BBTimerInvokeType.WhiffWindowTimer)]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BehaviorInfo))]
    public class WhiffWindowTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>();

            int currentOrder = -1;
            HashSetComponent<string> options = machine.GetParam<HashSetComponent<string>>("CancelWindow_Options");
            foreach (long infoId in machine.DescendInfoList)
            {
                BehaviorInfo info = machine.GetChild<BehaviorInfo>(infoId);
                if (options.Contains(info.behaviorName) && info.Trigger())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }

            if (currentOrder == -1) return;
            
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

    [Invoke(BBTimerInvokeType.TransitionWindowTimer)]
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(BehaviorInfo))]
    public class TransitionCancelWindowTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            //在过度动画取消窗口内，行为机逻辑上已经进入中立状态，可以切换进当前行为
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>();

            //1. 
            int currentOrder = machine.GetCurrentOrder();
            foreach (long infoId in machine.DescendInfoList)
            {
                BehaviorInfo info = machine.GetChild<BehaviorInfo>(infoId);
                if (info.behaviorOrder == 0 || info.moveType is MoveType.HitStun || info.moveType is MoveType.Etc)
                {
                    continue;
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
            
            //2.
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();

            long timer = machine.GetParam<long>("CancelWidow_Timer");
            bbTimer.Remove(ref timer);
            machine.TryRemoveParam("CancelWidow_Timer");
            
            //3. 
            machine.Reload(currentOrder);
        }
    }
    
    [Invoke(BBTimerInvokeType.DefaultWindowTimer)]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BehaviorMachine))]
    public class DefaultWindowTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>();

            //1. 
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
            
            //2.
            BBTimerComponent bbTimer = self.GetComponent<BBTimerComponent>();

            long timer = machine.GetParam<long>("CancelWidow_Timer");
            bbTimer.Remove(ref timer);
            machine.TryRemoveParam("CancelWidow_Timer");
            
            //3. 
            machine.Reload(currentOrder);
        }
    }
    
    public class CancelWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CancelWindow";
        }

        //CancelWindow: Gatling / Transition / Default / Whiff;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CancelWindow: (?<WindowType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            
            
            switch (match.Groups["WindowType"].Value)
            {
                case "Gatling":
                {
                    long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.GCWindowTimer, unit);
                    HashSetComponent<string> cancelOptions = HashSetComponent<string>.Create();
                    machine.RegistParam("CancelWindow_Timer", timer);
                    machine.RegistParam("CancelWindow_Options", cancelOptions);   
                    break;
                }
                case "Whiff":
                {
                    long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.WhiffWindowTimer, unit);
                    HashSetComponent<string> cancelOptions = HashSetComponent<string>.Create();
                    machine.RegistParam("CancelWindow_Timer", timer);
                    machine.RegistParam("CancelWindow_Options", cancelOptions);   
                    break;
                }
                case "Transition":
                {
                    long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.TransitionWindowTimer, unit);
                    machine.RegistParam("CancelWindow_Timer", timer);
                    break;
                }
                case "Default":
                {
                    long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.DefaultWindowTimer, unit);
                    machine.RegistParam("CancelWindow_Timer", timer);
                    break;
                }
                default:
                {
                    return Status.Failed;
                }
            }
            
            token.Add(() =>
            {
                if (machine.ContainParam("CancelWindow_Timer"))
                {
                    long _timer = machine.GetParam<long>("CancelWindow_Timer");
                    bbTimer.Remove(ref _timer);
                    machine.TryRemoveParam("CancelWindow_Timer");
                }
                if (machine.ContainParam("CancelWindow_Options"))
                {
                    HashSetComponent<string> options = machine.GetParam<HashSetComponent<string>>("CancelWindow_Options");
                    options.Dispose();
                    machine.TryRemoveParam("CancelWindow_Options");
                }
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}