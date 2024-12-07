namespace ET.Client
{
    [FriendOf(typeof(HitboxComponent))]
    public static class HitboxComponentSystem
    {
        public class HitboxComponentPostStepSystem : PostStepSystem<HitboxComponent>
        {
            protected override void PosStepUpdate(HitboxComponent self)
            {
                self.CollisionBuffer.Clear();
            }
        }
        
        public class HitboxComponentLoadSystem : LoadSystem<HitboxComponent>
        {
            protected override void Load(HitboxComponent self)
            {
                self.Init();
                //b2World创建刚体
                EventSystem.Instance.Invoke(new CreateB2bodyCallback(){instanceId = self.GetParent<TimelineComponent>().InstanceId});
            }
        }
        
        public static void Init(this HitboxComponent self)
        {
            self.keyFrame = null;
            self.CollisionBuffer.Clear();
        }
    }
}