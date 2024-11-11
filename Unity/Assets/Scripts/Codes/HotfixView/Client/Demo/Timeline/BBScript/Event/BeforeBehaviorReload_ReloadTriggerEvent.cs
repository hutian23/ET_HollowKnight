namespace ET.Client
{
    [Event(SceneType.Client)]
    public class BeforeBehaviorReload_ReloadTriggerEvent : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            hitboxComponent.ClearTriggerEvent();
            
            await ETTask.CompletedTask;
        }
    }
}