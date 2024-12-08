using System.Collections.Generic;
using System.Diagnostics;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BBTimerManager : Entity,IAwake,IDestroy, IUpdate, IFrameUpdate, ILoad
    {
        [StaticField]
        public static BBTimerManager Instance;

        public List<long> instanceIds = new(); //管理当前场景下的帧计时器
        public Stopwatch _gameTimer = new();
        public long LastTime;
    }
}