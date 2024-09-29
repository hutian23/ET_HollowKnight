﻿using Testbed.Abstractions;

namespace ET.Client
{
    public static class GameManagerSystem
    {
        public class GameManagerAwakesystem: AwakeSystem<GameManager>
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
        
        public class GameManagerUpdateSystem: UpdateSystem<GameManager>
        {
            protected override void Update(GameManager self)
            {
                TimelineManager.Instance.Update();
                Global.Settings.SingleStep = false;
            }
        }
        
        public static void Reload(this GameManager self)
        {
            //Editor
            Global.Settings.Pause = false;
            Global.Settings.SingleStep = false;
            
            //2. b2World reload
            b2GameManager.Instance.Reload();
            //1. timeline
            TimelineManager.Instance.Reload();
        }
    }
}