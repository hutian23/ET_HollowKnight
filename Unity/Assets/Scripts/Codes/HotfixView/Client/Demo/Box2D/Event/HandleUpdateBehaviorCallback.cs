namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BehaviorInfo))]
    public class HandleUpdateBehaviorCallback : AInvokeHandler<UpdateBehaviorCallback>
    {
        public override void Handle(UpdateBehaviorCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            BehaviorMachine machine = timelineComponent.GetComponent<BehaviorMachine>();
            BehaviorInfo info = machine.GetInfoByName(args.behaviorName);
            timelineComponent.Reload(info.behaviorOrder);
        }
    }
}