using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2WorldManager))]
    public class HandlePausedModeCallback : AInvokeHandler<PausedCallback>
    {
        public override void Handle(PausedCallback args)
        {
            //1. BBTimer
            BBTimerManager.Instance.Pause(args.Pause);
            //2. b2World
            Global.Settings.Pause = args.Pause;
        }
    }
}