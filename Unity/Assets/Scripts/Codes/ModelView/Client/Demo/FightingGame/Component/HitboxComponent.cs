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
        public Queue<CollisionInfo> CollisionBuffer = new();
    }
}