using Box2DSharp.Dynamics;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(b2Body))]
    public class BeforeBehaviorReload_DummyReloadB2Body : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            b2Body b2Body = b2GameManager.Instance.GetBody(unit.InstanceId);
            HitboxComponent hitboxComponent = unit.GetComponent<TimelineComponent>().GetComponent<HitboxComponent>();

            //1. 销毁旧夹具
            for (int i = 0; i < b2Body.hitBoxFixtures.Count; i++)
            {
                Fixture fixture = b2Body.hitBoxFixtures[i];
                b2Body.body.DestroyFixture(fixture);
            }
            b2Body.hitBoxFixtures.Clear();

            await ETTask.CompletedTask;
        }
    }
}