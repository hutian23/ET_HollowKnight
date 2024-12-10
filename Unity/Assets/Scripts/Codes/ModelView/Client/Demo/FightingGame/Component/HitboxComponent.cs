using System.Collections.Generic;
using System.Numerics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class HitboxComponent: Entity, IAwake, IDestroy, IPostStep, ILoad
    {
        public long bodyId; 
        public HitboxKeyframe keyFrame;// 判定框关键帧
        public Queue<CollisionInfo> CollisionBuffer = new(); // 当前帧收集碰撞信息
    }
    
    public struct CreateB2bodyCallback
    {
        public long instanceId;
    }
}