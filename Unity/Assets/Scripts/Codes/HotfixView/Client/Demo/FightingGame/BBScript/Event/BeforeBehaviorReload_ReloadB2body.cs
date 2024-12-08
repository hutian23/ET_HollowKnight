namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(InputWait))]
    [FriendOf(typeof(HitboxComponent))]
    public class BeforeBehaviorReload_ReloadB2body : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            HitboxComponent hitboxComponent = unit.GetComponent<TimelineComponent>().GetComponent<HitboxComponent>();
            
            //1. 销毁旧夹具
            b2Body.ClearHitbox();
            //2. 更新hitbox
            hitboxComponent.Init();
            
            await ETTask.CompletedTask;
        }
    }
}