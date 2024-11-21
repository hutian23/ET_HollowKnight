using Testbed.Abstractions;

namespace ET.Client
{
    [Event((SceneType.Client))]
    [FriendOf(typeof(BBTimerManager))]
    public class PausedCallback_HandleBBTimer : AEvent<PausedCallback>
    {
        protected override async ETTask Run(Scene scene, PausedCallback args)
        {
            foreach (long instanceId in BBTimerManager.Instance.instanceIds)
            {
                BBTimerComponent bbTimer = Root.Instance.Get(instanceId) as BBTimerComponent;
                if (Global.Settings.Pause)
                {
                    bbTimer.Pause();
                }
                else
                {
                    bbTimer.Restart();
                }
            }
            await ETTask.CompletedTask;
        }
    }
}