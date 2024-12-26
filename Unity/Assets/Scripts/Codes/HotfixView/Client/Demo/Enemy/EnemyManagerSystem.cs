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
    }
}