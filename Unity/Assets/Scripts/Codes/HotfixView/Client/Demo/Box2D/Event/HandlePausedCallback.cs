using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2GameManager))]
    public class HandlePausedModeCallback : AInvokeHandler<PausedCallback>
    {
        public override void Handle(PausedCallback args)
        {
            Global.Settings.Pause = args.Pause;

            //1. BBTimer
            BBTimerManager.Instance.Pause(args.Pause);
            
            //2. b2GameManager
            b2GameManager.Instance.Paused = args.Pause;
        }
    }
}