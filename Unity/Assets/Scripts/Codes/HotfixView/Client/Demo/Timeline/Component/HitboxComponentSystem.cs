using Box2DSharp.Dynamics.Contacts;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.HitboxCheckTimer)]
    [FriendOf(typeof(HitboxComponent))]
    public class HitboxCheckTimer : BBTimer<HitboxComponent>
    {
        protected override void Run(HitboxComponent self)
        {
        }
    }

    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(TriggerEvent))]
    public static class HitboxComponentSystem
    {
        public static void Init(this HitboxComponent self)
        {
            self.keyFrame = null;
            self.contactQueue.Clear();
        }

        public static void EnqueueContact(this HitboxComponent self, Contact contact)
        {
            if (self.contactQueue.Count >= HitboxComponent.MaxContactCount)
            {
                self.contactQueue.Dequeue();
            }
            self.contactQueue.Enqueue(contact);
        }
    }
}