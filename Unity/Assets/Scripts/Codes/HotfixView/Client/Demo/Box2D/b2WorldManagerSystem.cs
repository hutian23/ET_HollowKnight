using Box2DSharp.Dynamics;
using ET.Event;
using Testbed.Abstractions;
using Camera = UnityEngine.Camera;

namespace ET.Client
{
    [FriendOf(typeof (b2WorldManager))]
    [FriendOf(typeof (b2Body))]
    public static class b2WorldManagerSystem
    {
        public class b2WorldManagerAwakeSystem: AwakeSystem<b2WorldManager>
        {
            protected override void Awake(b2WorldManager self)
            {
                b2WorldManager.Instance = self;
                self.PreStepTimer = self.AddChild<BBTimerComponent>().Id;
                self.PostStepTimer = self.AddChild<BBTimerComponent>().Id;
                self.Reload();
            }
        }

        public class b2WorldManagerDestroySystem: DestroySystem<b2WorldManager>
        {
            protected override void Destroy(b2WorldManager self)
            {
                b2WorldManager.Instance = null;
                self.Init();
            }
        }
        
        public class b2WorldManagerLoadSystem : LoadSystem<b2WorldManager>
        {
            protected override void Load(b2WorldManager self)
            {
                self.Reload();
            }
        }

        public class b2WorldManagerPreStepSystem : PreStepSystem<b2WorldManager>
        {
            protected override void PreStepUpdate(b2WorldManager self)
            {
                int count = self.BodyQueue.Count;
                while (count-- > 0)
                {
                    long instanceId = self.BodyQueue.Dequeue();
                    //映射的unit已经销毁,销毁对应刚体
                    Unit unit = Root.Instance.Get(instanceId) as Unit;
                    if (unit == null || unit.InstanceId == 0)
                    {
                        b2Body body = self.GetBody(unit.InstanceId);
                        self.DestroyBody(body.Id);
                        continue;
                    }
                    self.BodyQueue.Enqueue(instanceId);
                }
            }
        }

        private static void Init(this b2WorldManager self)
        {
            self.Game = null;
            self.B2World?.Dispose();

            foreach (var kv in self.BodyDict)
            {
                b2Body body = self.GetChild<b2Body>(kv.Value);
                body.Dispose();
            }
            self.BodyDict.Clear();
            self.BodyQueue.Clear();
            
            //reload timer
            BBTimerComponent PreStepTimer = self.GetChild<BBTimerComponent>(self.PreStepTimer);
            BBTimerComponent PostStepTimer = self.GetChild<BBTimerComponent>(self.PostStepTimer);
            PreStepTimer.Reload();
            PostStepTimer.Reload();
            
            Global.Settings.Pause = false;
            Global.Settings.SingleStep = false;
        }
        
        private static void Reload(this b2WorldManager self)
        {
            self.Init();
            self.Game = Camera.main.GetComponent<b2Game>();
            self.B2World = new b2World(self.Game);
            EventSystem.Instance.PublishAsync(self.DomainScene(), new AfterB2WorldCreated() { B2World = self.B2World }).Coroutine();
        }

        #region B2body

        public static b2Body CreateBody(this b2WorldManager self, long unitId, BodyDef bodyDef)
        {
            if (self.BodyDict.ContainsKey(unitId))
            {
                Log.Error($"already exist b2body with unitId: {unitId}");
                return null;
            }

            b2Body b2Body = b2WorldManager.Instance.AddChild<b2Body>();
            b2Body.body = b2WorldManager.Instance.B2World.World.CreateBody(bodyDef);
            b2Body.unitId = unitId;
            self.BodyDict.TryAdd(b2Body.unitId, b2Body.Id);
            self.BodyQueue.Enqueue(b2Body.unitId);
            return b2Body;
        }
        
        /// <summary>
        /// 通过Unit.InstanceId查找对应的b2Body
        /// </summary>
        public static b2Body GetBody(this b2WorldManager self, long instanceId)
        {
            if (!self.BodyDict.TryGetValue(instanceId, out long id))
            {
                Log.Error($"does not exist b2Body, unit.InstanceId: {instanceId}");
                return null;
            }
            return self.GetChild<b2Body>(id);
        }

        /// <summary>
        /// 移除B2Body, 该方法不能在B2World.isLocked时调用
        /// </summary>
        /// <param name="self"></param>
        /// <param name="instanceId">Unit.InstanceId</param>
        private static void DestroyBody(this b2WorldManager self, long instanceId)
        {
            b2Body b2Body = self.GetBody(instanceId);
            if (b2Body == null)
            {
                return;
            }
            self.B2World.World.DestroyBody(b2Body.body);
            b2Body.Dispose();
            self.BodyDict.Remove(instanceId);
        }

        /// <summary>
        /// 激活刚体
        /// </summary>
        public static void EnableBody(this b2WorldManager self, long instanceId,bool enable)
        {
            if (!self.BodyDict.ContainsKey(instanceId))
            {
                return;
            }
            b2Body b2Body = self.GetBody(instanceId);
            b2Body.SetEnable(enable);
        }
        #endregion
        
        public static void Step(this b2WorldManager self)
        {
            self.B2World.Step();
        }
        
        public static BBTimerComponent GetPreStepTimer(this b2WorldManager self)
        {
            return self.GetChild<BBTimerComponent>(self.PreStepTimer);
        }

        public static BBTimerComponent GetPostStepTimer(this b2WorldManager self)
        {
            return self.GetChild<BBTimerComponent>(self.PostStepTimer);
        }

        public static bool IsLocked(this b2WorldManager self)
        {
            return self.B2World.IsLocked;
        }
    }
}