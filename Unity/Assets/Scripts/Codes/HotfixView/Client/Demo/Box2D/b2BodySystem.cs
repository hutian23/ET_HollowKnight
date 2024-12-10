using Box2DSharp.Dynamics;
using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;
using Transform = Box2DSharp.Common.Transform;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public static class b2BodySystem
    {
        public class b2BodyDestroySystem: DestroySystem<b2Body>
        {
            protected override void Destroy(b2Body self)
            {
                self.unitId = 0;
                self.hitBoxFixtures.Clear();
                self.body = null;
                self.Flip = FlipState.Left;
            }
        }
        
        public class B2bodyPreStepSystem : PreStepSystem<b2Body>
        {
            protected override void PreStepUpdate(b2Body self)
            {
                Log.Warning(self.GetVelocity()+"  "+"Prestep");
            }
        }
        
        public class B2bodyPostStepSystem : PostStepSystem<b2Body>
        {
            protected override void PosStepUpdate(b2Body self)
            {
                Log.Warning(self.GetVelocity()+"  "+ "PostStep");
                //未发生位置更新，渲染层无需刷新
                Transform curTrans = self.body.GetTransform();
                if (self.trans.Equals(curTrans))
                {
                    return;
                }

                //同步渲染层GameObject和逻辑层b2World中刚体的位置旋转信息
                self.trans = curTrans;
                Unit unit = Root.Instance.Get(self.unitId) as Unit;
                GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
                Vector2 position = curTrans.Position.ToUnityVector2();
                Vector3 axis = new(0, 0, curTrans.Rotation.Angle * Mathf.Rad2Deg);

                go.transform.position = position;
                go.transform.eulerAngles = axis;
                go.transform.localScale = new Vector3(self.GetFlip(), 1, 1);
            }
        }

        public static System.Numerics.Vector2 GetVelocity(this b2Body self)
        {
            return self.body.LinearVelocity;
        }

        public static void SetVelocity(this b2Body self, System.Numerics.Vector2 value)
        {
            self.body.SetLinearVelocity(value);
        }

        public static void SetVelocityX(this b2Body self, float velocityX)
        {
            System.Numerics.Vector2 oldVel = self.body.LinearVelocity;
            System.Numerics.Vector2 newVel = new(-velocityX * self.GetFlip(), oldVel.Y);
            self.SetVelocity(newVel);
        }

        public static void SetVelocityY(this b2Body self, float velocityY)
        {
            System.Numerics.Vector2 oldVel = self.body.LinearVelocity;
            System.Numerics.Vector2 newVel = new(oldVel.X, velocityY);
            self.SetVelocity(newVel);
        }

        public static void SetFlip(this b2Body self, FlipState flipState)
        {
            self.Flip = flipState;
        }

        public static int GetFlip(this b2Body self)
        {
            return (int)self.Flip;
        }

        public static void SetPosition(this b2Body self, System.Numerics.Vector2 position)
        {
            self.body.SetTransform(position, 0f);
        }

        public static System.Numerics.Vector2 GetPosition(this b2Body self)
        {
            return self.body.GetPosition();
        }

        public static void ClearHitbox(this b2Body self)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot dispose fixture while b2World is locked!!");
                return;
            }
            for (int i = 0; i < self.hitBoxFixtures.Count; i++)
            {
                Fixture fixture = self.hitBoxFixtures[i];
                self.body.DestroyFixture(fixture);
            }
            self.hitBoxFixtures.Clear();
        }

        public static void CreateHitbox(this b2Body self,FixtureDef fixtureDef)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot dispose fixture while b2World is locked!!");
                return;
            }
            Fixture fixture = self.body.CreateFixture(fixtureDef);
            self.hitBoxFixtures.Add(fixture);
        }
    }
}