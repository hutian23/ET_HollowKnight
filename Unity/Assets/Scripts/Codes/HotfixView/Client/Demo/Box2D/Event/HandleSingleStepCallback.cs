using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(TimelineManager))]
    [FriendOf(typeof(BBTimerComponent))]
    public class HandleSingleStepCallback : AInvokeHandler<SingleStepCallback>
    {
        public override void Handle(SingleStepCallback args)
        {
            if (!Global.Settings.Pause)
            {
                return;
            }
            
            // //1. 更新timer
            // foreach (long instanceId in TimelineManager.Instance.instanceIds)
            // {
            //     TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
            //     //1. combat timer
            //     // 关闭 / 启动 当前场景下所有unit的战斗计时器
            //     BBTimerComponent combatTimer = timelineComponent.GetComponent<BBTimerComponent>();
            //     combatTimer.Accumulator += combatTimer.GetFrameLength();
            //     combatTimer.TimerUpdate();
            //
            //     //2. input timer(if unit is player)
            //     InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            //     if (inputWait == null) continue;
            //     BBTimerComponent inputTimer = inputWait.GetComponent<BBTimerComponent>();
            //     inputTimer.Accumulator += inputTimer.GetFrameLength();
            //     inputTimer.TimerUpdate();
            // }
            //
            // //2. 物理层更新一帧
            // b2GameManager.Instance.Step();
        }
    }
}