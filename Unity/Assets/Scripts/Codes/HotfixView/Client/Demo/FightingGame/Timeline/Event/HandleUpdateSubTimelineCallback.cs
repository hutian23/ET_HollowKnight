using Timeline;

namespace ET.Client
{
    [Invoke]
    public class HandleUpdateSubTimelineCallback : AInvokeHandler<UpdateSubTimelineCallback>
    {
        public override void Handle(UpdateSubTimelineCallback args)
        {
            Log.Warning("Update SubTimeline Track");
            
        }
    }
}