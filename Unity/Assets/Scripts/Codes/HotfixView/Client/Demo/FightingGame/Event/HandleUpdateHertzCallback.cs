using Timeline;

namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(TimelineComponent))]
    [FriendOf(typeof(BBTimerComponent))]
    public class HandleUpdateHertzCallback : AInvokeHandler<UpdateHertzCallback>
    {
        public override void Handle(UpdateHertzCallback args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            if (timelineComponent == null || timelineComponent.InstanceId == 0)
            {
                Log.Error($"timelineComponent has already disposed!!!");
                return;
            }
            
            //1. 显示层更新Hertz
            timelineComponent.GetTimelinePlayer().Hertz = args.Hertz;

            //2. 逻辑帧更新TimeScale
            Unit unit = timelineComponent.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            
            bbTimer.SetHertz(args.Hertz);
            bbTimer.Accumulator = 0;

            // 速度随timeScale缩放
            b2Unit.SetHertz(args.Hertz);
        }
    }
}