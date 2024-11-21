using System.Collections.Generic;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class HitboxComponent: Entity, IAwake, IDestroy
    {
        public HitboxKeyframe keyFrame;

        //当前帧收集碰撞信息
        public Queue<CollisionInfo> infoQueue = new();
        
        //当前行为注册的碰撞回调
        public HashSet<string> callbackSet = new();
    }
}