using Testbed.Abstractions;

namespace ET.Client
{
    [FriendOf(typeof(GameManager))]
    public static class GameManagerSystem
    {
        public class GameManagerAwakeSystem : AwakeSystem<GameManager>
        {
            protected override void Awake(GameManager self)
            {
                GameManager.Instance = self;
            }
        }

        public class GameManagerLoadSystem : LoadSystem<GameManager>
        {
            protected override void Load(GameManager self)
            {
                self.Reload();
            }
        }
        
        public class GameManagerDestroySystem: DestroySystem<GameManager>
        {
            protected override void Destroy(GameManager self)
            {
                TimerComponent.Instance.Remove(ref self.SingleTimer);
            }
        }

        [FriendOf(typeof(b2GameManager))]
        public class GameManagerFixedUpdateSystem : FixedUpdateSystem<GameManager>
        {
            protected override void FixedUpdate(GameManager self)
            {
                BBInputComponent.Instance.FixedUpdate();
                TimelineManager.Instance.FixedUpdate();
                b2GameManager.Instance.FixedUpdate();
            }
        }

        public static void Reload(this GameManager self)
        {
            //Editor
            Global.Settings.Pause = false;
            Global.Settings.SingleStep = false;

            //4. reload global timer
            BBTimerManager.Instance.SceneTimer().ReLoad();
            //1. b2World reload
            b2GameManager.Instance.Reload();
            //2. timeline
            TimelineManager.Instance.Reload();
            //3. reload input
            BBInputComponent.Instance.Reload();
        }
    }
}