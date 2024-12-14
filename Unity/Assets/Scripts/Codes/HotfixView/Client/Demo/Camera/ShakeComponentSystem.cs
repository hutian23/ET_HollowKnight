using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;

namespace ET.Client
{
    public static class ShakeComponentSystem
    {
        public class ShakeComponentDestroySystem : DestroySystem<ShakeComponent>
        {
            protected override void Destroy(ShakeComponent self)
            {
                self.UnitId = 0;
                self.ShakeCnt = 0;
                self.TotalShakeCnt = 0;
                self.ShakeLength = 0;
            }
        }
        
        public class ShakeComponentPostStepSystem : PostStepSystem<ShakeComponent>
        {
            protected override void PosStepUpdate(ShakeComponent self)
            {
                if (self.ShakeCnt <= 0)
                {
                    self.Dispose();
                    return;
                }
                
                Unit unit = Root.Instance.Get(self.UnitId) as Unit;
                b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
                GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
                
                Vector3 shakePos = new Vector3(
                    self.Random.Next(-self.ShakeLength, self.ShakeLength),
                    self.Random.Next(-self.ShakeLength, self.ShakeLength),
                    0) / 1000f;
                Vector3 Pos = b2Body.GetPosition().ToUnityVector3();

                go.transform.position = Pos + shakePos * self.ShakeCnt / self.TotalShakeCnt;

                self.ShakeCnt--;
            }
        }
    }
}