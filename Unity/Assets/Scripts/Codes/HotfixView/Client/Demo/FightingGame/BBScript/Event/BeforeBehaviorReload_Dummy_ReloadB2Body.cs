namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(b2Body))]
    public class BeforeBehaviorReload_Dummy_ReloadB2Body : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            b2Unit b2Unit = unit.GetComponent<TimelineComponent>().GetComponent<b2Unit>();

            //1. 销毁旧夹具
            b2Body.ClearHitbox();
            //2. 更新hitbox
            b2Unit.Init();

            await ETTask.CompletedTask;
        }
    }
}