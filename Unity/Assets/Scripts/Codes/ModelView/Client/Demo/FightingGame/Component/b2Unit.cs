using System.Collections.Generic;
using System.Numerics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class b2Unit: Entity, IAwake, IDestroy, IPostStep, ILoad, IPreStep
    {
        public HitboxKeyframe keyFrame;// 判定框关键帧
        public Queue<CollisionInfo> CollisionBuffer = new(); // 当前帧收集碰撞信息
        public Vector2 Velocity; // 和刚体的实际速度区分
        public int Hertz = 60; // 根据TimeScale对速度进行缩放 Hertz / 60
    }
    
    public struct CreateB2bodyCallback
    {
        public long instanceId;
    }
}