namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(TriggerEvent))]
    public static class HitboxComponentSystem
    {
        public static void Init(this HitboxComponent self)
        {
            self.keyFrame = null;
            self.callbackSet.Clear();
            self.infoQueue.Clear();
        }
    }
}