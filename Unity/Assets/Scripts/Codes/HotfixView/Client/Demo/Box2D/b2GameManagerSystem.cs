using ET.Event;
using Camera = UnityEngine.Camera;

namespace ET.Client
{
    [FriendOf(typeof (b2GameManager))]
    [FriendOf(typeof (b2Body))]
    public static class b2GameManagerSystem
    {
        public class b2WorldManagerAwakeSystem: AwakeSystem<b2GameManager>
        {
            protected override void Awake(b2GameManager self)
            {
                b2GameManager.Instance = self;
                self.PreStepTimer = self.AddChild<BBTimerComponent>().Id;
                self.PostStepTimer = self.AddChild<BBTimerComponent>().Id;
                self.Reload();
            }
        }

        public class b2WorldManagerDestroySystem: DestroySystem<b2GameManager>
        {
            protected override void Destroy(b2GameManager self)
            {
                b2GameManager.Instance = null;
                self.Game = null;
                self.B2World?.Dispose();
                self.BodyDict.Clear();
            }
        }
        
        public class b2WorldManagerPostStepSystem : PostStepSystem<b2GameManager>
        {
            protected override void PosStepUpdate(b2GameManager self)
            {
                foreach (var kv in self.BodyDict)
                {
                    b2Body b2Body = self.GetChild<b2Body>(kv.Value);
                    b2Body.PostStep();
                }
                self.GetPostStepTimer().Step();
            }
        }

        public static void Reload(this b2GameManager self)
        {
            self.Game = Camera.main.GetComponent<b2Game>();
            
            //create new b2World
            self.B2World?.Dispose();
            self.B2World = new b2World(self.Game);
            self.Paused = false;
            
            //dispose b2body
            foreach (var kv in self.BodyDict)
            {
                b2Body body = self.GetChild<b2Body>(kv.Value);
                body.Dispose();
            }
            self.BodyDict.Clear();

            //reload timer
            BBTimerComponent PreStepTimer = self.GetChild<BBTimerComponent>(self.PreStepTimer);
            BBTimerComponent PostStepTimer = self.GetChild<BBTimerComponent>(self.PostStepTimer);
            PreStepTimer.ReLoad();
            PostStepTimer.ReLoad();
            
            //create hitbox
            EventSystem.Instance.PublishAsync(self.DomainScene(), new AfterB2WorldCreated() { B2World = self.B2World }).Coroutine();
        }
        
        public static void FixedUpdate(this b2GameManager self)
        {
            if (self.Paused) return;
            self.Step();
        }

        public static void Step(this b2GameManager self)
        {
            self.B2World.Step();
        }
        

        public static b2Body GetBody(this b2GameManager self, long unitId)
        {
            if (!self.BodyDict.TryGetValue(unitId, out long id))
            {
                Log.Error($"does not exist b2Body, unitId: {unitId}");
                return null;
            }

            return self.GetChild<b2Body>(id);
        }

        public static BBTimerComponent GetPreStepTimer(this b2GameManager self)
        {
            return self.GetChild<BBTimerComponent>(self.PreStepTimer);
        }

        public static BBTimerComponent GetPostStepTimer(this b2GameManager self)
        {
            return self.GetChild<BBTimerComponent>(self.PostStepTimer);
        }

        public static bool IsLocked(this b2GameManager self)
        {
            return self.B2World.IsLocked;
        }
    }
}