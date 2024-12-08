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
            BehaviorBuffer buffer = self.GetComponent<BehaviorBuffer>();
            BBParser parser = self.GetComponent<BBParser>();
            BehaviorInfo info = buffer.GetInfoByOrder(behaviorOrder);
            
            // 切换行为前回调
            EventSystem.Instance.PublishAsync(self.ClientScene(), new BeforeBehaviorReload() { instanceId = self.GetParent<Unit>().InstanceId, behaviorOrder = behaviorOrder }).Coroutine();
            
            //渲染层 重新生成PlayableGraph
            self.GetTimelinePlayer().Init(info.Timeline);
            
            // 调用Main协程
            parser.Init(info.opDict);
            parser.Invoke(info.GetFunctionPointer("Main"), parser.CancellationToken).Coroutine();
        }
    }
}