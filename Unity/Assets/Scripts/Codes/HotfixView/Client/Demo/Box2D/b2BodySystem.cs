using Box2DSharp.Dynamics;
using Box2DSharp.Testbed.Unity.Inspection;
using Timeline;
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
                self.Fixtures.Clear();
                self.FixtureDict.Clear();
                self.trans = default;
                self.Flip = FlipState.Left;
                self.UpdateFlag = false;
            }
        }
        
        public class B2bodyPostStepSystem : PostStepSystem<b2Body>
        {
            protected override void PosStepUpdate(b2Body self)
            {
                //static body
                if (self.unitId == 0)
                {
                    return;
                }
                
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
        /// 激活刚体
        /// 刚体处于未激活状态下，不会参与碰撞、射线检测、查询
        /// 未激活刚体仍然可以创建夹具、关节
        /// </summary>
        public static void SetEnable(this b2Body self, bool isEnable)
        {
            self.body.IsEnabled = isEnable;
        }
        
        /// <summary>
        /// 刚体朝向
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

        #region Fixture
        /// <summary>
        /// 取消行为时，销毁hitbox
        /// </summary>
        /// <param name="self"></param>
        public static void ClearHitBoxes(this b2Body self)
        {
            //销毁hitbox
            QueueComponent<Fixture> removeQueue = QueueComponent<Fixture>.Create();
            for (int i = 0; i < self.Fixtures.Count; i++)
            {
                Fixture fixture = self.Fixtures[i];
                FixtureData data = (FixtureData)fixture.UserData;
                if (data.Type is FixtureType.Hitbox)
                {
                    removeQueue.Enqueue(fixture);
                }
            }
            int count = removeQueue.Count;
            while (count-- > 0)
            {
                Fixture fixture = removeQueue.Dequeue();
                self.DestroyFixture(fixture);
            }
            removeQueue.Dispose();
        }
        
        /// <summary>
        /// 热重载时调用，销毁所有夹具
        /// </summary>
        /// <param name="self"></param>
        public static void ClearFixtures(this b2Body self)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot dispose fixture while b2World is locked!!");
                return;
            }
            for (int i = 0; i < self.Fixtures.Count; i++)
            {
                Fixture fixture = self.Fixtures[i];
                self.body.DestroyFixture(fixture);
            }
            self.Fixtures.Clear();
            self.FixtureDict.Clear();
        }

        public static bool DestroyFixture(this b2Body self, string fixtureName)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot destroy fixture while b2World is locked!!");
                return false;
            }

            if (!self.FixtureDict.TryGetValue(fixtureName, out Fixture fixture))
            {
                Log.Error($"not found fixture: {fixtureName}");
                return false;
            }

            self.Fixtures.Remove(fixture);
            self.FixtureDict.Remove(fixtureName);
            self.body.DestroyFixture(fixture);
            return true;
        }

        public static bool DestroyFixture(this b2Body self, Fixture fixture)
        {
            FixtureData data = (FixtureData)fixture.UserData;
            self.DestroyFixture(data.Name);
            return true;
        }
        
        public static void CreateFixture(this b2Body self,FixtureDef fixtureDef)
        {
            if (b2WorldManager.Instance.IsLocked())
            {
                Log.Error($"cannot create fixture while b2World is locked!!");
                return;
            }
            FixtureData data = (FixtureData)fixtureDef.UserData;
            if (self.FixtureDict.ContainsKey(data.Name))
            {
                Log.Error($"already contain fixture!, name: {data.Name}");
                return;
            }
            
            Fixture fixture = self.body.CreateFixture(fixtureDef);
            self.Fixtures.Add(fixture);
            self.FixtureDict.Add(data.Name, fixture);
        }
        #endregion
    }
}