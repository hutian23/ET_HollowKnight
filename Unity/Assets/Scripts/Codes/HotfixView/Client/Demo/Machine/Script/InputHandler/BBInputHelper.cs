namespace ET.Client
{
    [FriendOf(typeof(BehaviorMachine))]
    [FriendOf(typeof(InputWait))]
    public static class BBInputHelper
    {
        public static long GetBuffFrame(this InputWait self, int buffFrame)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            return bbTimer.GetNow() + buffFrame;
        }
    }
}