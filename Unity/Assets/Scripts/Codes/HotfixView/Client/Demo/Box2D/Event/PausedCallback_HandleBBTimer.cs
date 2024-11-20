using Testbed.Abstractions;

namespace ET.Client
{
    [Event((SceneType.Client))]
    [FriendOf(typeof(TimelineManager))]
    public class PausedCallback_HandleBBTimer : AEvent<PausedCallback>
    {
        protected override async ETTask Run(Scene scene, PausedCallback args)
        {
            foreach (long instanceId in TimelineManager.Instance.instanceIds)
            {
                TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
                
                //1. combat timer
                // 关闭 / 启动 当前场景下所有unit的战斗计时器
                BBTimerComponent combatTimer = timelineComponent.GetComponent<BBTimerComponent>();
                if (Global.Settings.Pause)
                {
                    combatTimer.Pause();
                }
                else
                {
                    combatTimer.Restart();
                }
                
                //2. input timer(if unit is player)
                InputWait inputWait = timelineComponent.GetComponent<InputWait>();
                if (inputWait == null)
                {
                    continue;
                }
                BBTimerComponent inputTimer = inputWait.GetComponent<BBTimerComponent>();
                if (Global.Settings.Pause)
                {
                    inputTimer.Pause();
                }
                else
                {
                    inputTimer.Restart();
                }

            }
            await ETTask.CompletedTask;
        }
    }
}