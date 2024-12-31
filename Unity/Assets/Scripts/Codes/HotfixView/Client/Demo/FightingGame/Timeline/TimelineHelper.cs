using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(BBParser))]
    public static class TimelineHelper
    {
        public static void Reload(this TimelineComponent self, string behaviorName)
        {
            BehaviorBuffer buffer = self.GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetInfoByName(behaviorName);
            self.Reload(info.behaviorOrder);
        }
        
        public static void Reload(this TimelineComponent self, int behaviorOrder)
        {
            BBParser parser = self.GetComponent<BBParser>();
            BehaviorBuffer buffer = self.GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetInfoByOrder(behaviorOrder);
            
            // 1. 切换行为前回调, 物理层回调 + 逻辑层回调
            EventSystem.Instance.PublishAsync(self.DomainScene(), new BeforeBehaviorReload()
            {
                instanceId = self.GetParent<Unit>().InstanceId, 
                behaviorOrder = behaviorOrder
            }).Coroutine();
            
            // 2. 调用Main协程
            parser.Invoke(parser.GetFunctionPointer(info.behaviorName, "Main"), parser.CancellationToken).Coroutine();
        }

        public static bool Trigger(this BehaviorInfo self)
        {
            TimelineComponent timelineComponent = self.GetParent<BehaviorBuffer>().GetParent<TimelineComponent>();
            BBParser parser = timelineComponent.GetComponent<BBParser>();

            //不存在函数
            int index = parser.GetFunctionPointer(self.behaviorName, "Trigger");
            if (index < 0)
            {
                return false;
            }
            
            for (int i = index + 1; i < parser.OpDict.Count; i++)
            {
                string opLine = parser.OpDict[i];
                if (opLine.Equals("return;"))
                {
                    return true;
                }
                Match match = Regex.Match(opLine, @"^\w+");
                if (!match.Success)
                {
                    DialogueHelper.ScripMatchError(opLine);
                    return false;
                }
                
                //执行TriggerHandler
                BBTriggerHandler handler = ScriptDispatcherComponent.Instance.GetTrigger(match.Value);
                BBScriptData data = BBScriptData.Create(opLine, 0, 0);
                bool ret = handler.Check(parser, data);
                if (ret is false)
                {
                    return false;
                }
            }
            
            return true;
        }
    }
}