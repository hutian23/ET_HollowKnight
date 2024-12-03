using System;

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

        public static void Reload(this BBTimerManager self)
        {
            self._gameTimer.Restart();
            self.LastTime = 0;
            
            self.SceneTimer().Reload();
            foreach (long instanceId in self.instanceIds)
            {
                BBTimerComponent bbTimer = Root.Instance.Get(instanceId) as BBTimerComponent;
                bbTimer.Reload();
            }
        }
        
        public static BBTimerComponent SceneTimer(this BBTimerManager self)
        {
            BBTimerComponent sceneTimer = Root.Instance.Get(self.SceneTimerId) as BBTimerComponent;
            return sceneTimer;
        }

        public static void RegistSceneTimer(this BBTimerManager self, BBTimerComponent sceneTimer)
        {
            self.SceneTimerId = sceneTimer.InstanceId;
        }
        
        //管理timer
        public static void RegistTimer(this BBTimerManager self, BBTimerComponent bbTimer)
        {
            self.instanceIds.Add(bbTimer.InstanceId);
        }

        public static void RemoveTimer(this BBTimerManager self, BBTimerComponent bbTimer)
        {
            self.instanceIds.Remove(bbTimer.InstanceId);
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