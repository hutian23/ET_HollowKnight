using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBTimerManager))]
    [FriendOf(typeof(b2GameManager))]
    public class HandlePausedModeCallback : AInvokeHandler<PausedCallback>
    {
        public override void Handle(PausedCallback args)
        {
            Global.Settings.Pause = args.Pause;

            //1. BBTimer
            foreach (long instanceId in BBTimerManager.Instance.instanceIds)
            {
                BBTimerComponent bbTimer = Root.Instance.Get(instanceId) as BBTimerComponent;
                if (Global.Settings.Pause)
                {
                    bbTimer.Pause();
                }
                //没有添加卡肉效果
                else if (!BBTimerManager.Instance.FrozenIds.Contains(bbTimer.InstanceId))
                {
                    bbTimer.Restart();
                }
            }

            //2. b2GameManager
            b2GameManager.Instance.Paused = args.Pause;
        }
    }
}