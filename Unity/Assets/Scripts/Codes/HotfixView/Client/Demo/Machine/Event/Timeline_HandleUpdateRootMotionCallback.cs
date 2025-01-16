﻿using System.Numerics;
using Box2DSharp.Testbed.Unity.Inspection;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof (b2Body))]
    public class Timeline_HandleUpdateRootMotionCallback: AInvokeHandler<UpdateRootMotionCallback>
    {
        public override void Handle(UpdateRootMotionCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Unit unit = timelineComponent.GetParent<Unit>();
            BBParser parser = unit.GetComponent<BBParser>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            
            if (!args.ApplyRootMotion)
            {
                parser.TryRemoveParam("ApplyRootMotion");
                return;
            }

            parser.TryRemoveParam("ApplyRootMotion");
            parser.RegistParam("ApplyRootMotion", true);
            //因为资源中默认朝向为左,横向速度需要翻转
            b2Unit.SetVelocity(args.velocity.ToVector2() * new Vector2(-1,1));
        }
    }
}