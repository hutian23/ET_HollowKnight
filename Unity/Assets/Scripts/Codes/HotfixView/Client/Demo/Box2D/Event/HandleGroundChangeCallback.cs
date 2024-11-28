namespace ET.Client
{
    [Invoke]
    public class HandleGroundChangeCallback : AInvokeHandler<OnGroundChanged>
    {
        public override void Handle(OnGroundChanged args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;

            long maxJump = timelineComponent.GetParam<long>("MaxJump");
            timelineComponent.UpdateParam("JumpCount", maxJump);
        }
    }
}