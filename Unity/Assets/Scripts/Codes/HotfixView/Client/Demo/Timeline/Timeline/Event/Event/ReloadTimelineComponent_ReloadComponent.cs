﻿namespace ET.Client
{
    [Event(SceneType.Current)]
    [FriendOf(typeof(InputWait))]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
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

            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BBParser parser = timelineComponent.GetComponent<BBParser>();

            //1. 初始化
            buffer.Init();
            
            //1-1 RootInit
            string RootScript = buffer.GetParent<TimelineComponent>().GetTimelinePlayer().BBPlayable.rootScript;
            parser.InitScript(RootScript);
            await parser.Invoke("RootInit", parser.cancellationToken);
            if(parser.cancellationToken.IsCancel()) return;
            
            //4. 启动检测定时器
            buffer.CheckTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.BehaviorCheckTimer, buffer);
            #endregion

            //重载Parser,进入默认行为
            BehaviorInfo info = buffer.GetInfoByOrder(0);
            timelineComponent.Reload(info.Timeline,info.behaviorOrder);
            
            //注册变量
            timelineComponent.RegistParam("OnGround", false);
            
            await ETTask.CompletedTask;
        }
    }
}