namespace ET.Client
{
    [Invoke]
    public class HandleThroneControllerCallback : AInvokeHandler<ThronesControllerCallback>
    {
        public override void Handle(ThronesControllerCallback args)
        {
            Scene curScene = ClientSceneManagerComponent.Instance.Get(1).CurrentScene();
            UnitComponent unitComponent = curScene.GetComponent<UnitComponent>();
            
            Unit unit = unitComponent.AddChild<Unit, int>(1001);
            unit.AddComponent<GameObjectComponent>().GameObject = args.controller.gameObject;
            args.controller.InstanceId = unit.InstanceId;
            
            unit.AddComponent<BBTimerComponent>().IsFrameUpdateTimer();
            unit.AddComponent<BBParser, int>(ProcessType.ThroneControllerProcess).Start();
        }
    }
}