using System;
using System.Collections.Generic;
using Box2DSharp.Dynamics;
using Transform = Box2DSharp.Common.Transform;

namespace ET.Client
{
    [ChildOf(typeof (b2GameManager))]
    public class b2Body: Entity, IAwake, IDestroy
    {
        //记录unit的instanceId
        public long unitId;
        public Body body;

        //当刚体的位置信息没有发生更新，但是刚体的其他属性更新时，可以调用这个成员通知更新显示层
        public bool UpdateFlag;

        //当前步长，b2World中刚体的位置转换信息
        public Transform trans;

        //当前帧建立的hitBox
        public List<Fixture> hitBoxFixtures = new();

        public List<Fixture> fixtures = new();
        
        public FlipState Flip = FlipState.Left;
        public int Gravity;
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

    public struct AfterSyncTransform
    {
        public long instanceId;
    }
}