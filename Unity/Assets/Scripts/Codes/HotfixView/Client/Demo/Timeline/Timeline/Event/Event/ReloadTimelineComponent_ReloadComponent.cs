using Timeline;

namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(InputWait))]
    [FriendOf(typeof(SkillBuffer))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(SkillInfo))]
    public class ReloadTimelineComponent_ReloadComponent : AEvent<ReloadTimelineComponent>
    {
        protected override async ETTask Run(Scene scene, ReloadTimelineComponent args)
        {
            //解决热重载时，组件调用顺序的问题
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            timelineComponent.Init();
            
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            bbTimer.ReLoad();

            //获得输入，更新输入缓冲区定时器
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            inputWait.Init();
            inputWait.StartInputHandleTimer();

            //重载行为机
            #region SkillBuffer

            SkillBuffer buffer = timelineComponent.GetComponent<SkillBuffer>();
            BBParser parser = timelineComponent.GetComponent<BBParser>();

            //1. 初始化
            buffer.Init();

            var timelines = buffer.GetParent<TimelineComponent>()
                    .GetTimelinePlayer().BBPlayable
                    .GetTimelines();
            
            //1-1 RootInit
            string RootScript = buffer.GetParent<TimelineComponent>().GetTimelinePlayer().BBPlayable.rootScript;
            parser.InitScript(RootScript);
            await parser.Invoke("RootInit", parser.cancellationToken);
            if(parser.cancellationToken.IsCancel()) return;
            
            foreach (BBTimeline timeline in timelines)
            {
                SkillInfo info = buffer.AddChild<SkillInfo>();

                //1. 加载Trigger
                info.LoadSkillInfo(timeline);

                //2. 初始化协程
                parser.InitScript(timeline.Script);
                parser.RegistParam("InfoId", info.Id);
                await parser.Invoke("Init", parser.cancellationToken);
                if (parser.cancellationToken.IsCancel()) return;
            }

            //3. 按照优先级注册行为到InfoDict中,权值越高的行为越前检查
            foreach (Entity child in buffer.Children.Values)
            {
                SkillInfo info = child as SkillInfo;
                buffer.infoDict.Add(info.behaviorOrder, info.Id);
                buffer.behaviorMap.Add(info.behaviorName, info.behaviorOrder);
            }

            //4. 启动检测定时器
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.BehaviorCheckTimer, buffer);
            #endregion

            //重载Parser
            timelineComponent.Reload(0);
            
            await ETTask.CompletedTask;
        }
    }
}