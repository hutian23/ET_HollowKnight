namespace ET.Client
{
    [ComponentOf(typeof(TimelineComponent))]
    public class TimelineScripProcessor : Entity, IAwake,IDestroy, ILoad
    {
    }

    public struct TimelineScriptCallback
    {
        public long instanceId;
    }
}