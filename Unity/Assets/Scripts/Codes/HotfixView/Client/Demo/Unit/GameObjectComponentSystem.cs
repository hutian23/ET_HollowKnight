namespace ET.Client
{
    public static class GameObjectComponentSystem
    {
        [ObjectSystem]
        public class DestroySystem: DestroySystem<GameObjectComponent>
        {
            protected override void Destroy(GameObjectComponent self)
            {
                //池化对象则返回对象池
                if (self.GameObject != null && self.GameObject.GetComponent<PoolObject>() != null)
                {
                    GameObjectPoolHelper.ReturnObjectToPool(self.GameObject);
                    return;
                }
                UnityEngine.Object.Destroy(self.GameObject);
            }
        }
    }
}