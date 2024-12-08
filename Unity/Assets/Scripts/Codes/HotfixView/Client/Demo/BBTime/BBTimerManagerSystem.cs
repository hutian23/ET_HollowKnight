using System;
using Testbed.Abstractions;

namespace ET.Client
{
    [FriendOf(typeof(BBTimerManager))]
    public static class BBTimerManagerSystem
    {
        public class BBTimerManagerAwakeSystem : AwakeSystem<BBTimerManager>
        {
            protected override void Awake(BBTimerManager self)
            {
                BBTimerManager.Instance = self;
            }
        }
        
        public class BBTimerManagerUpdateSystem : UpdateSystem<BBTimerManager>
        {
            protected override void Update(BBTimerManager self)
            {
                long now = self._gameTimer.ElapsedTicks;
                long Accumulator = now - self.LastTime;
                self.LastTime = now;
                Accumulator = (long)(Accumulator * (Global.Settings.Hertz / 60f));
                
                //1. SceneTimer
                self.SceneTimer().SceneTimerUpdate(Accumulator);
                
                //2. timeline Timer
                foreach (long instanceId in self.instanceIds)
                {
                    BBTimerComponent bbTimer = Root.Instance.Get(instanceId) as BBTimerComponent;
                    bbTimer?.TimerUpdate(Accumulator);
                }
            }
        }
        
        public class BBTimerManagerReloadSystem : LoadSystem<BBTimerManager>
        {
            protected override void Load(BBTimerManager self)
            {
                self.Reload();
            }
        }
        
        public static void Step(this BBTimerManager self)
        {
            //1. SceneTimer
            self.SceneTimer().SceneTimerUpdate(TimeSpan.FromSeconds(1 / 60f).Ticks);
            
            //2. timeline timer
            foreach (long instanceId in self.instanceIds)
            {
                BBTimerComponent bbTimer = Root.Instance.Get(instanceId) as BBTimerComponent;
                bbTimer?.TimerUpdate(TimeSpan.FromSeconds(1/60f).Ticks);
            }
        }

        private static void Reload(this BBTimerManager self)
        {
            self._gameTimer.Restart();
            self.LastTime = 0;
            self.instanceIds.Clear();
        }
        
        public static BBTimerComponent SceneTimer(this BBTimerManager self)
        {
            BBTimerComponent sceneTimer = self.GetParent<Scene>().GetComponent<BBTimerComponent>();
            return sceneTimer;
        }
        
        //管理timer
        public static void RegistTimer(this BBTimerManager self, long instanceId)
        {
            self.instanceIds.Add(instanceId);
        }

        public static void RemoveTimer(this BBTimerManager self, long instanceId)
        {
            self.instanceIds.Remove(instanceId);
        }
        
        public static void Pause(this BBTimerManager self,bool pause)
        {
            if (pause)
            {
                self._gameTimer.Stop();   
            }
            else
            {
                self._gameTimer.Start();
            }
        }
    }
}