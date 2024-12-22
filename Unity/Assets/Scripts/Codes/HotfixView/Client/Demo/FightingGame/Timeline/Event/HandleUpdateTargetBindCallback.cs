using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleUpdateTargetBindCallback : AInvokeHandler<UpdateTargetBindCallback>
    {
        public override void Handle(UpdateTargetBindCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            Log.Warning("Update Target Bind");
        }
    }
}