﻿using System.Numerics;
using Box2DSharp.Collision.Shapes;
using Box2DSharp.Dynamics;
using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Unit))]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(b2WorldManager))]
    //HitboxTrack的回调
    public class HandleUpdateHitBoxCallback : AInvokeHandler<UpdateHitboxCallback>
    {
        public override void Handle(UpdateHitboxCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);

            if (args.Keyframe == null)
            {
                return;
            }
            //更新关键帧
            b2Unit.keyFrame = args.Keyframe;
            //1. 销毁旧的夹具
            b2Body.ClearHitBoxes();
            //2. 更新hitbox
            foreach (BoxInfo info in args.Keyframe.boxInfos)
            {
                PolygonShape shape = new();
                shape.SetAsBox(info.size.x / 2, info.size.y / 2, new Vector2(info.center.x * b2Body.GetFlip(), info.center.y), 0f);
                FixtureDef fixtureDef = new()
                {
                    Shape = shape,
                    Density = 1.0f,
                    Friction = 0f,
                    UserData = new FixtureData()
                    {
                        InstanceId = b2Body.InstanceId,
                        Name = info.boxName,
                        Type = FixtureType.Hitbox,
                        LayerMask = LayerType.Unit,
                        IsTrigger = info.hitboxType is not HitboxType.Squash,
                        UserData = info,
                        TriggerStayId = TriggerStayType.CollisionEvent,
                    },
                };
                b2Body.CreateFixture(fixtureDef);
            }
        }
    }
}