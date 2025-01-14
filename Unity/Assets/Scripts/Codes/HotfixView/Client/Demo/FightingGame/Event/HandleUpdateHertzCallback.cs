﻿using Timeline;

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
            if (timelineComponent == null) return;

            int hertz = (int)args.Hertz;
            timelineComponent.Hertz = hertz;

            // 显示层更新
            timelineComponent.GetTimelinePlayer().Hertz = hertz;

            // 逻辑帧更新TimeScale
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            bbTimer.SetHertz(hertz);
            bbTimer.Accumulator = 0;

            // 速度随timeScale缩放
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();
            b2Unit.SetHertz(hertz);
        }
    }
}