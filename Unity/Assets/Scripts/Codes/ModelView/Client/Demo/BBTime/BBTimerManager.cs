using System.Collections.Generic;

namespace ET.Client
{
    [ComponentOf(typeof(Scene))]
    public class BBTimerManager : Entity,IAwake,IDestroy
    {
        [StaticField]
        public static BBTimerManager Instance;
        
        public long SceneTimer_InstanceId;
        
        //管理当前场景下的帧计时器
        public List<long> instanceIds = new();

        //管理卡肉中的定时器
        //notes: 编辑器阶段 paused singleStep等功能会和卡肉效果冲突
        public List<long> FrozenIds = new();
    }
}