namespace ET.Client
{
    public static class EnemyManagerSystem
    {
        public class EnemyManagerAwakeSystem : AwakeSystem<EnemyManager>
        {
            protected override void Awake(EnemyManager self)
            {
                EnemyManager.Instance = self;
            }
        }
        
        public class EnemyManagerLoadSystem : LoadSystem<EnemyManager>
        {
            protected override void Load(EnemyManager self)
            {
                int count = self.ReloadQueue.Count;
                while (count -- > 0)
                {
                    long instanceId = self.ReloadQueue.Dequeue();
                    Unit unit = Root.Instance.Get(instanceId) as Unit;
                    unit?.Dispose();
                }
            }
        }
    }
}