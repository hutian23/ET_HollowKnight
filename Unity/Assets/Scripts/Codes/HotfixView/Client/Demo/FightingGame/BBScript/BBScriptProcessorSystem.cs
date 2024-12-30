using System.Collections.Generic;

namespace ET.Client
{
    [FriendOf(typeof(BBScripProcessor))]
    public static class BBScriptProcessorSystem
    {
        public class BBScriptProcessorAwakeSystem : AwakeSystem<BBScripProcessor>
        {
            protected override void Awake(BBScripProcessor self)
            {
                EventSystem.Instance.Invoke(new ReloadBBScriptCallback() { instanceId = self.InstanceId });
            }
        }

        public class BBScriptProcessorLoadSystem : LoadSystem<BBScripProcessor>
        {
            protected override void Load(BBScripProcessor self)
            {
                // 抛出事件
                EventSystem.Instance.Invoke(new ReloadBBScriptCallback() { instanceId = self.InstanceId });
            }
        }

        public static int GetGroupIndex(this BBScripProcessor self, string groupName)
        {
            return self.groupDict.GetValueOrDefault(groupName, -1);
        }
    }
}