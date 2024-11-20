using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BBTimerManager : Entity,IAwake,IDestroy
    {
        [StaticField]
        public static BBTimerManager Instance;

        //管理当前场景下的帧计时器
        public List<long> instanceId = new();
    }
}