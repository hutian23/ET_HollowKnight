namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(TriggerEvent))]
    public static class HitboxComponentSystem
    {
        public class HitboxComponentPostStepSystem : PostStepSystem<HitboxComponent>
        {
            protected override void PosStepUpdate(HitboxComponent self)
            {
                self.CollisionBuffer.Clear();
            }
        }
        
        public static void Init(this HitboxComponent self)
        {
            self.keyFrame = null;
            self.CollisionBuffer.Clear();
        }
    }
}