﻿using System.Collections.Generic;
using System.Diagnostics;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BBTimerManager : Entity,IAwake,IDestroy, IUpdate, IFrameUpdate
    {
        [StaticField]
        public static BBTimerManager Instance;
        //管理当前场景下的帧计时器
        public List<long> instanceIds = new();
        public long SceneTimerId = 0;
        
        public Stopwatch _gameTimer = new();
        public long LastTime;
    }
}