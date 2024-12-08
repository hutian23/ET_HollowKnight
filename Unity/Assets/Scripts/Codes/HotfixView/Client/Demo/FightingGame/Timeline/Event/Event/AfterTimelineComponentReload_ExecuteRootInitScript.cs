namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(BBParser))]
    public class AfterTimelineComponentReload_ExecuteRootInitScript : AEvent<AfterTimelineComponentReload>
    {
        protected override async ETTask Run(Scene scene, AfterTimelineComponentReload args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            
            //1-1 RootInit
            // string RootScript = timelineComponent.GetTimelinePlayer().BBPlayable.rootScript;
            //
            // await parser.Invoke(1, parser.CancellationToken);
            // if (parser.CancellationToken.IsCancel()) return;
            
            //1-2 重载Parser,进入默认行为
            // timelineComponent.Reload(0);
            Log.Warning("Hello World");
            await ETTask.CompletedTask;
        }
    }
}