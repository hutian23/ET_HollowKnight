using ET.Event;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.HitboxCheckTimer)]
    [FriendOf(typeof(HitboxComponent))]
    public class HitboxCheckTimer : BBTimer<HitboxComponent>
    {
        protected override void Run(HitboxComponent self)
        {
            self.infoQueue.Clear();
        }
    }

    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(TriggerEvent))]
    public static class HitboxComponentSystem
    {
        public static void Init(this HitboxComponent self)
        {
            self.keyFrame = null;
            self.infoQueue.Clear();
        }

        public static void EnqueueInfo(this HitboxComponent self, CollisionInfo info)
        {
            if (self.infoQueue.Count >= HitboxComponent.MaxContactCount)
            {
                self.infoQueue.Dequeue();
            }
            self.infoQueue.Enqueue(info);
        }
    }
}