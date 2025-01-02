using Box2DSharp.Dynamics;
using UnityEngine;

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
            
            //3. 存在b2Box子物体，生成夹具(行为机切换行为时不会销毁，热重载时销毁)
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject; 
            Log.Warning(go.GetComponentsInChildren<b2Box>().Length.ToString());
        }
    }
}