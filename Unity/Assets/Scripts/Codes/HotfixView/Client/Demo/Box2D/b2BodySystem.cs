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
                self.body = null;
                self.unitId = 0;
                self.hitBoxFixtures.Clear();
                self.trans = default;
                self.Flip = FlipState.Left;
                self.UpdateFlag = false;
            }
        }
        
        public class B2bodyPostStepSystem : PostStepSystem<b2Body>
        {
            protected override void PosStepUpdate(b2Body self)
            {
                //未发生更新，渲染层无需刷新
                Transform curTrans = self.body.GetTransform();
                if (self.trans.Equals(curTrans) && !self.UpdateFlag)
                {
                    return;
                }
                self.UpdateFlag = false;

                //同步渲染层GameObject和逻辑层b2World中刚体的位置旋转信息
                self.trans = curTrans;
                Unit unit = Root.Instance.Get(self.unitId) as Unit;
                GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
                Vector3 position = curTrans.Position.ToUnityVector3();
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
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="flipState"></param>
        /// <param name="refresh">需要翻转hitbox</param>
        public static void SetFlip(this b2Body self, FlipState flipState, bool refresh = false)
        {
            if (self.Flip != flipState && refresh)
            {
                EventSystem.Instance.Invoke(new UpdateFlipCallback(){instanceId = self.unitId,curFlip = flipState});
            }
            self.Flip = flipState;
            self.UpdateFlag = true;
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