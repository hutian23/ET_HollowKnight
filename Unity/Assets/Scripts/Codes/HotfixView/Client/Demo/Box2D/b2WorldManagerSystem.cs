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
        
        public class B2WorldManagerFixedUpdateSystem : FixedUpdateSystem<b2WorldManager>
        {
            protected override void FixedUpdate(b2WorldManager self)
            {
                if (Global.Settings.Pause) return;
                self.Step();
            }
        }
        
        public class b2WorldManagerPostStepSystem : PostStepSystem<b2WorldManager>
        {
            protected override void PosStepUpdate(b2WorldManager self)
            {
                foreach (var kv in self.BodyDict)
                {
                    b2Body b2Body = self.GetChild<b2Body>(kv.Value);
                    b2Body.PostStep();
                }
                self.GetPostStepTimer().Step();
            }
        }
        
        public class b2WorldManagerLoadSystem : LoadSystem<b2WorldManager>
        {
            protected override void Load(b2WorldManager self)
            {
                self.Reload();
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

        public static void Step(this b2WorldManager self)
        {
            self.B2World.Step();
        }
        
        public static b2Body GetBody(this b2WorldManager self, long unitId)
        {
            if (!self.BodyDict.TryGetValue(unitId, out long id))
            {
                Log.Error($"does not exist b2Body, unitId: {unitId}");
                return null;
            }

            return self.GetChild<b2Body>(id);
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