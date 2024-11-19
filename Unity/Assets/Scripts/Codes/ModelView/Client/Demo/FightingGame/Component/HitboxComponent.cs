using System.Collections.Generic;
using Box2DSharp.Dynamics.Contacts;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class HitboxComponent: Entity, IAwake, IDestroy
    {
        public HitboxKeyframe keyFrame;

        //记录当前帧Hitbox的碰撞信息
        public Queue<CollisionInfo> infoQueue = new(MaxContactCount);

        public long checkTimer;
        public const int MaxContactCount = 100;
    }
}