using Timeline;

namespace ET.Client
{
    [Invoke]
    public class Timeline_HandleUpdateHertzCallback : AInvokeHandler<UpdateHertzCallback>
    {
        public override void Handle(UpdateHertzCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null || timelineComponent.InstanceId == 0)
            {
                Log.Error($"cannot found TimelineComponent: {args.instanceId}");
                return;
            }

            Unit unit = timelineComponent.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            EventSystem.Instance.Invoke(new BehaviorUpdateHertzCallback(){instanceId = machine.InstanceId,hertz = args.Hertz});
        }
    }
}