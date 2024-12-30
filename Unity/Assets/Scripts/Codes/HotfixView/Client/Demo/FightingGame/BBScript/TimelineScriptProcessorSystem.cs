namespace ET.Client
{
    [FriendOf(typeof(TimelineScripProcessor))]
    public static class TimelineScriptProcessorSystem
    {
        public class BBScriptProcessorAwakeSystem : AwakeSystem<TimelineScripProcessor>
        {
            protected override void Awake(TimelineScripProcessor self)
            {
                EventSystem.Instance.Invoke(new TimelineScriptCallback() { instanceId = self.InstanceId });
            }
        }

        public class BBScriptProcessorLoadSystem : LoadSystem<TimelineScripProcessor>
        {
            protected override void Load(TimelineScripProcessor self)
            {
                // 抛出事件
                EventSystem.Instance.Invoke(new TimelineScriptCallback() { instanceId = self.InstanceId });
            }
        }
    }
}