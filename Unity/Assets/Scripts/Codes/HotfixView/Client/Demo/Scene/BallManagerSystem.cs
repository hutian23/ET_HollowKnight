namespace ET.Client
{
    public static class BallManagerSystem
    {
        public class BallManagerAwakeSystem : AwakeSystem<BallManager>
        {
            protected override void Awake(BallManager self)
            {
                BallManager.Instance = self;
            }
        }
        
        public class BallManagerLoadSystem : LoadSystem<BallManager>
        {
            protected override void Load(BallManager self)
            {
                int count = self.InstanceIds.Count;
                while (count -- > 0)
                {
                    long instanceId = self.InstanceIds.Dequeue();
                    Unit unit = Root.Instance.Get(instanceId) as Unit;
                    if (unit == null) continue;
                    unit.Dispose();
                }
            }
        }
    }
}