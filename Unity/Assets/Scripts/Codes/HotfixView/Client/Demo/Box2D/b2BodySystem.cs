using Box2DSharp.Dynamics;
using Box2DSharp.Testbed.Unity.Inspection;
using UnityEngine;
using Transform = Box2DSharp.Common.Transform;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(RootMotionComponent))]
    public static class b2BodySystem
    {
        public class b2BodyDestroySystem : DestroySystem<b2Body>
        {
            protected override void Destroy(b2Body self)
            {
                self.unitId = 0;
                self.hitBoxFixtures.Clear();
                self.body = null;
                self.Flip = FlipState.Left;
                self.UpdateFlag = false;
                self.Gravity = 0;
            }
        }

        public static void PostStep(this b2Body self)
        {
            Unit unit = Root.Instance.Get(self.unitId) as Unit;

            Transform curTrans = self.body.GetTransform();
            if (self.trans.Equals(curTrans) && !self.UpdateFlag)
            {
                return;
            }

            //同步渲染层GameObject和逻辑层b2World中刚体的位置旋转信息
            self.trans = curTrans;
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            Vector2 position = curTrans.Position.ToUnityVector2();
            Vector3 axis = new(0, 0, curTrans.Rotation.Angle * Mathf.Rad2Deg);

            go.transform.position = position;
            go.transform.eulerAngles = axis;
            go.transform.localScale = new Vector3(self.GetFlip(), 1, 1);
            self.UpdateFlag = false;
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
            var oldVel = self.body.LinearVelocity;
            var newVel = new System.Numerics.Vector2(-velocityX * self.GetFlip(), oldVel.Y);
            self.body.SetLinearVelocity(newVel);
        }

        public static void SetVelocityY(this b2Body self, float velocityY)
        {
            var oldVel = self.body.LinearVelocity;
            var newVel = new System.Numerics.Vector2(oldVel.X, velocityY);
            self.body.SetLinearVelocity(newVel);
        }

        public static void SetFlip(this b2Body self, FlipState flipState)
        {
            self.Flip = flipState;
        }

        public static int GetFlip(this b2Body self)
        {
            return (int)self.Flip;
        }

        public static void SetUpdateFlag(this b2Body self)
        {
            self.UpdateFlag = true;
        }

        public static void RemoveUpdateFlag(this b2Body self)
        {
            self.UpdateFlag = false;
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
            if (b2GameManager.Instance.IsLocked())
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
            if (b2GameManager.Instance.IsLocked())
            {
                Log.Error($"cannot dispose fixture while b2World is locked!!");
                return;
            }
            Fixture fixture = self.body.CreateFixture(fixtureDef);
            self.hitBoxFixtures.Add(fixture);
        }
    }
}