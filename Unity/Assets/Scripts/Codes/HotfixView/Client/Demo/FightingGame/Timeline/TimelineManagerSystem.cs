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
                self.instanceIds.Clear();
            }
        }

        public static void FixedUpdate(this TimelineManager self)
        {
        }

        public class TimelineManagerDestroySystem: DestroySystem<TimelineManager>
        {
            protected override void Destroy(TimelineManager self)
            {
                TimelineManager.Instance = null;
                self.instanceIds.Clear();
            }
        }

        public static void Reload(this TimelineManager self)
        {
            foreach (long instanceId in self.instanceIds)
            {
                EventSystem.Instance.PublishAsync(self.DomainScene(), new ReloadTimelineComponent() { instanceId = instanceId }).Coroutine();
            }
        }
    }
}