using System;
using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(TimelineManager))]
    [FriendOf(typeof(BBTimerComponent))]
    [FriendOf(typeof(BBTimerManager))]
    public class HandleSingleStepCallback : AInvokeHandler<SingleStepCallback>
    {
        public override void Handle(SingleStepCallback args)
        {
            if (!Global.Settings.Pause)
            {
                return;
            }

            //1. 更新timer
            foreach (long instanceId in BBTimerManager.Instance.instanceIds)
            {
                BBTimerComponent bbTimer = Root.Instance.Get(instanceId) as BBTimerComponent;
                if (BBTimerManager.Instance.FrozenIds.Contains(bbTimer.InstanceId))
                {
                    continue;
                }
                
                long tick = TimeSpan.FromSeconds(1 / 60f).Ticks;
                bbTimer.Accumulator += tick;
                bbTimer.TimerUpdate();
            }

            //2. 物理层更新一帧
            b2GameManager.Instance.Step();
        }
    }
}