namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    public static class HitboxComponentSystem
    {
        public static void ClearEvent(this HitboxComponent self)
        {
            foreach (long triggerEvent in self.triggerEventIds)
            {
                self.RemoveChild(triggerEvent);
            }
            self.triggerEventIds.Clear();
            self.HitboxDict.Clear();
        }

        public static bool ContainTriggerEvent(this HitboxComponent self, string hitboxName)
        {
            return self.HitboxDict.ContainsKey(hitboxName);
        }
        
        public static TriggerEvent GetTriggerEvent(this HitboxComponent self,string hitboxName)
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