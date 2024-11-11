namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(TriggerEvent))]
    public static class HitboxComponentSystem
    {
        public static void ClearTriggerEvent(this HitboxComponent self)
        {
            foreach (long triggerEvent in self.triggerEventIds)
            {
                self.RemoveChild(triggerEvent);
            }
            self.triggerEventIds.Clear();
            self.HitboxDict.Clear();
        }

        public static void ClearRunningTriggerEventCoroutine(this HitboxComponent self)
        {
            foreach (long parserId in self.parserIds)
            {
                self.RemoveChild(parserId);
            }
            self.parserIds.Clear();
        }

        public static void Init(this HitboxComponent self)
        {
            self.ClearTriggerEvent();
            self.ClearRunningTriggerEventCoroutine();
        }

        public static bool ContainTriggerEvent(this HitboxComponent self, string hitboxName, TriggerType triggerType)
        {
            if (!self.HitboxDict.TryGetValue(hitboxName, out long triggerId))
            {
                return false;
            }

            TriggerEvent triggerEvent = self.GetChild<TriggerEvent>(triggerId);
            return triggerEvent.TriggerType == triggerType;

        }

        public static TriggerEvent GetTriggerEvent(this HitboxComponent self, string hitboxName)
        {
            if (!self.HitboxDict.TryGetValue(hitboxName, out long id))
            {
                Log.Error($"does not exist triggerEvent: {hitboxName}");
                return null;
            }

            TriggerEvent triggerEvent = self.GetChild<TriggerEvent>(id);
            return triggerEvent;
        }
    }
}