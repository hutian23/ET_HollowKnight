namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(BBParser))]
    public class ReloadTimelineComponent_ReloadComponent : AEvent<ReloadTimelineComponent>
    {
        protected override async ETTask Run(Scene scene, ReloadTimelineComponent args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            //清空等待事件
            timelineComponent.RemoveComponent<ObjectWait>();
            timelineComponent.AddComponent<ObjectWait>();
            
            //1-1 RootInit
            // string RootScript = timelineComponent.GetTimelinePlayer().BBPlayable.rootScript;
            //
            // await parser.Invoke(1, parser.CancellationToken);
            // if (parser.CancellationToken.IsCancel()) return;
            
            //1-2 重载Parser,进入默认行为
            // timelineComponent.Reload(0);
            await ETTask.CompletedTask;
        }
    }
}