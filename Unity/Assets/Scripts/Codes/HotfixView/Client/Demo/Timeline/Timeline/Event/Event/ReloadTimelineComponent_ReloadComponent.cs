namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(InputWait))]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(HitboxComponent))]
    public class ReloadTimelineComponent_ReloadComponent : AEvent<ReloadTimelineComponent>
    {
        protected override async ETTask Run(Scene scene, ReloadTimelineComponent args)
        {
            //解决热重载时，组件调用顺序的问题
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBParser parser = timelineComponent.GetComponent<BBParser>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            
            timelineComponent.Init();
            bbTimer.ReLoad();

            //获得输入，更新输入缓冲区定时器, 只有玩家会挂载inputWait
            inputWait?.Init();

            //清空碰撞事件组件
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            hitboxComponent.Init();

            //清空等待事件
            timelineComponent.RemoveComponent<ObjectWait>();
            timelineComponent.AddComponent<ObjectWait>();

            //重载行为机
            //1-0 初始化
            buffer.Init();
            
            //1-1 RootInit
            string RootScript = buffer.GetParent<TimelineComponent>().GetTimelinePlayer().BBPlayable.rootScript;
            parser.InitScript(RootScript);
            await parser.Invoke("RootInit", parser.cancellationToken);
            if (parser.cancellationToken.IsCancel()) return;
            
            //1-2 重载Parser,进入默认行为
            timelineComponent.Reload(0);
        }
    }
}