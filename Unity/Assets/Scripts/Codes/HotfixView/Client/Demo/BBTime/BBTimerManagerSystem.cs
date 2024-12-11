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
                self.Reload();
            }
        }
        
        // 21462  166666
        public class BBTimerManagerUpdateSystem : UpdateSystem<BBTimerManager>
        {
            protected override void Update(BBTimerManager self)
            {
                long now = self._gameTimer.ElapsedTicks;
                long Accumulator = now - self.LastTime;
                self.LastTime = now;

                self.SceneTimer().SetHertz((int)(Global.Settings.TimeScale * 60));
                self.Step(Accumulator);
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
            long Accumulator = self.SceneTimer().GetFrameLength();
            self.Step(Accumulator);
        }

        private static void Step(this BBTimerManager self, long Accumulator)
        {
            //执行顺序
            //1. 场景计时器的定时器和异步任务
            BBTimerComponent sceneTimer = self.SceneTimer();
            long preFrame = sceneTimer.GetNow();
            sceneTimer.TimerUpdate(Accumulator);
            long curFrame = sceneTimer.GetNow();
            Global.Settings.StepCount = curFrame;
            
            long Dt = curFrame - preFrame;
            while (Dt-- > 0)
            {
                //2. FrameUpdate 生命周期事件
                EventSystem.Instance.FrameUpdate();
                //3. Timeline相关 定时器和异步任务
                foreach (long instanceId in self.instanceIds)
                {
                    BBTimerComponent bbTimer = Root.Instance.Get(instanceId) as BBTimerComponent;
                    bbTimer?.TimerUpdate(166666);
                }
                //4. 物理层 PreStep PostStep生命周期事件
                b2WorldManager.Instance.Step();
            }
        }

        private static void Reload(this BBTimerManager self)
        {
            self._gameTimer.Restart();
            self.LastTime = self._gameTimer.ElapsedTicks;
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