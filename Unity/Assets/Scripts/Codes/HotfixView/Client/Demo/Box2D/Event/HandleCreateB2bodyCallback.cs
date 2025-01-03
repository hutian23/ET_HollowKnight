using Box2DSharp.Dynamics;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(b2WorldManager))]    
    //这个事件用于建立Unit和b2world中刚体的映射
    public class HandleCreateB2bodyCallback : AInvokeHandler<CreateB2bodyCallback>
    {
        public override void Handle(CreateB2bodyCallback args)
        {
            World world = b2WorldManager.Instance.B2World.World;

            //1. 物理层创建刚体
            BodyDef bodyDef = new()
            {
                BodyType = BodyType.DynamicBody,
                GravityScale = 0f,
                LinearDamping = 0f,
                AngularDamping = 0f,
                AllowSleep = true,
                FixedRotation = true,
            };
            Body body = world.CreateBody(bodyDef);
            
            //2. 建立Unit和刚体的映射关系
            b2Body b2Body = b2WorldManager.Instance.AddChild<b2Body>();
            b2Body.body = body;
            b2Body.unitId = args.instanceId;
            b2WorldManager.Instance.BodyDict.TryAdd(b2Body.unitId, b2Body.Id);
            
            //3. 子物体挂载b2Box脚本，生成夹具
            // Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            // GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            // foreach (b2Box box in go.GetComponentsInChildren<b2Box>())
            // {
            //     PolygonShape shape = new();
            //     shape.SetAsBox(box.info.size.x / 2, box.info.size.y / 2, box.info.center.ToVector2(), 0f);
            //     FixtureDef fixtureDef = new()
            //     {
            //         Shape = shape,
            //         Density = 1.0f,
            //         Friction = 0.0f,
            //         UserData = new FixtureData()
            //         {
            //             InstanceId = b2Body.InstanceId, // 代表SceneBox
            //             Name = box.info.boxName,
            //             Type = FixtureType.Default,
            //             LayerMask = LayerType.Unit,
            //             IsTrigger = box.IsTrigger,
            //             UserData = box.info,
            //             TriggerStayId = TriggerStayType.CollisionEvent
            //         }
            //     };
            //     b2Body.CreateFixture(fixtureDef);
            // }
        }
    }
}