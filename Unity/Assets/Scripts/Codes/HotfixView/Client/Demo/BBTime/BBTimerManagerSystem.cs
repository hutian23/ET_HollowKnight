namespace ET.Client
{
    [FriendOf(typeof(BBTimerManager))]
    public static class BBTimerManagerSystem
    {
        public class BBTimerManagerAwakeSystem : AwakeSystem<BBTimerManager>
        {
            protected override void Awake(BBTimerManager self)
            {
                BBTimerManager.Instance = self;
                self.instanceIds.Clear();
                self.SceneTimer_InstanceId = 0;
            }
        }

        public static BBTimerComponent SceneTimer(this BBTimerManager self)
        {
            BBTimerComponent SceneTimer = Root.Instance.Get(self.SceneTimer_InstanceId) as BBTimerComponent;
            return SceneTimer;
        }

        //管理timer
        public static void RegistTimer(this BBTimerManager self, BBTimerComponent bbTimer)
        {
            self.instanceIds.Add(bbTimer.InstanceId);
        }
    }
}