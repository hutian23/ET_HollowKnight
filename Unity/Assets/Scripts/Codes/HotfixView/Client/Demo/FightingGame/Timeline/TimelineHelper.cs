namespace ET.Client
{
    [FriendOf(typeof(BehaviorInfo))]
    public static class TimelineHelper
    {
        public static void Reload(this TimelineComponent self, string behaviorName)
        {
            BehaviorBuffer buffer = self.GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetInfoByName(behaviorName);
            self.Reload(info.Timeline, info.behaviorOrder);
        }

        public static void Reload(this TimelineComponent self, int behaviorOrder)
        {
            BehaviorBuffer buffer = self.GetComponent<BehaviorBuffer>();
            BehaviorInfo info = buffer.GetInfoByOrder(behaviorOrder);
            self.Reload(info.Timeline, info.behaviorOrder);
        }
    }
}