namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GCWindowTimer)]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BehaviorInfo))]
    public class GCWindowTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();

            //1. 处理输入
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            inputWait.InputNotify();

            //2. 处理行为
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            BehaviorInfo curInfo = buffer.GetInfoByOrder(buffer.currentOrder);
            foreach (long infoId in buffer.infoIds)
            {
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(infoId);
                if (info.behaviorOrder == curInfo.behaviorOrder)
                {
                    continue;
                }

                //2-1 加特林取消不能取消到该行为
                bool ret = (info.moveType > curInfo.moveType) || buffer.ContainGCOption(info.behaviorOrder);
                if (!ret)
                {
                    continue;
                }
                
                //2-2 检查trigger
                ret = info.BehaviorCheck();
                if (ret)
                {
                    timelineComponent.Reload(info.Timeline, info.behaviorOrder);
                }

                break;
            }
        }
    }

    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    //加特林取消窗口，招式之间相互取消
    public class OpenGCWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "OpenGCWindow";
        }

        //OpenGCWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.GCWindowTimer, parser);
            parser.RegistParam("GCWindowTimer", timer);
            parser.cancellationToken.Add(() =>
            {
                bbTimer.Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}