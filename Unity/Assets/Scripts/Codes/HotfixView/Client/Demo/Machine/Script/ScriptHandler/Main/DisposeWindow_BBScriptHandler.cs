namespace ET.Client
{
    [FriendOf(typeof(BehaviorMachine))]
    public class DisposeWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DisposeWindow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            
            // 销毁定时器
            long timer = machine.GetParam<long>("CancelWindow_Timer");
            bbTimer.Remove(ref timer);
            machine.TryRemoveParam("CancelWindow_Timer");
            
            // 回收集合
            if (machine.ContainParam("CancelWindow_Options"))
            {
                HashSetComponent<string> options = machine.GetParam<HashSetComponent<string>>("CancelWindow_Options");
                options.Dispose();
                machine.TryRemoveParam("CancelWindow_Options");
            }
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}