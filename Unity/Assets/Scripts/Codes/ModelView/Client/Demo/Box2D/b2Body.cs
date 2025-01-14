﻿using System.Collections.Generic;
using Box2DSharp.Dynamics;
using Timeline;
using Transform = Box2DSharp.Common.Transform;

namespace ET.Client
{
    // 管理物理层的刚体
    [ChildOf(typeof (b2WorldManager))]
    public class b2Body: Entity, IAwake, IDestroy, IPostStep, IPreStep, IFrameUpdate
    {
        public Body body;
        public long unitId; // 记录unit的instanceId
        public List<Fixture> Fixtures = new();
        public Dictionary<string, Fixture> FixtureDict = new(); // 方便通过夹具名称查询夹具
        public Transform trans; // 当前step中b2World中刚体的位置转换信息
        public FlipState Flip = FlipState.Left;
        public bool UpdateFlag; // 手动刷新渲染层
    }
    
    //转向后，更新夹具
    public struct UpdateFlipCallback
    {
        public long instanceId;
        public FlipState curFlip;
    }
}