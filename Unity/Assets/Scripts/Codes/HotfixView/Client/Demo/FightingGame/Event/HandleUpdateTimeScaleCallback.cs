using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleUpdateTimeScaleCallback : AInvokeHandler<UpdateTimeScaleCallback>
    {
        public override void Handle(UpdateTimeScaleCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            timelineComponent.SetHertz((int)args.hertz);
        }
    }
}