using System;
using System.Collections.Generic;
using Box2DSharp.Dynamics;
using Transform = Box2DSharp.Common.Transform;

namespace ET.Client
{
    [ChildOf(typeof (b2WorldManager))]
    public class b2Body: Entity, IAwake, IDestroy, IPostStep, IPreStep, IFrameUpdate
    {
        public Body body;
        
        public long unitId;     // 记录unit的instanceId
        public List<Fixture> hitBoxFixtures = new(); // 当前帧的判定框,更新关键帧(UpdateHitboxCallback/UpdateFlipCallback)时刷新这个列表
        
        public Transform trans; // 当前step中b2World中刚体的位置转换信息
        public FlipState Flip = FlipState.Left;
    }

    [Flags]
    public enum FlipState
    {
        Left = 1,
        Right = -1
    }

    //转向后，更新夹具
    public struct UpdateFlipCallback
    {
        public long instanceId;
        public FlipState curFlip;
    }
}