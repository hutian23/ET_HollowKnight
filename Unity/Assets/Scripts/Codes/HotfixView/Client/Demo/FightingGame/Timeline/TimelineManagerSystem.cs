namespace ET.Client
{
    [FriendOf(typeof (TimelineManager))]
    [FriendOf(typeof (BBTimerComponent))]
    public static class TimelineManagerSystem
    {
        public class TimelineManagerAwakeSystem: AwakeSystem<TimelineManager>
        {
            protected override void Awake(TimelineManager self)
            {
                TimelineManager.Instance = self;
            }
        }

        public class TimelineManagerDestroySystem: DestroySystem<TimelineManager>
        {
            protected override void Destroy(TimelineManager self)
            {
                TimelineManager.Instance = null;
            }
        }
        
        public static void FixedUpdate(this TimelineManager self)
        {
        }

        public static void Reload(this TimelineManager self)
        {
            
        }
    }
}