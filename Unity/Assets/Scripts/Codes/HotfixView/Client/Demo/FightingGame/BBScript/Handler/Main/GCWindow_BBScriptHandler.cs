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
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            int currentOrder = -1;
            BehaviorInfo curInfo = buffer.GetInfoByOrder(buffer.GetCurrentOrder());
            foreach (long infoId in buffer.DescendInfoList)
            {
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(infoId);
                if (info.moveType is MoveType.HitStun || info.moveType is MoveType.Etc)
                {
                    continue;
                }

                if ((info.moveType > curInfo.moveType || buffer.ContainGCOption(info.behaviorOrder)) && info.BehaviorCheck())
                {
                    currentOrder = info.behaviorOrder;
                    break;
                }
            }
            
            if (currentOrder == -1)
            {
                return;
            }
            
            //初始化
            buffer.DisposeWindow();
            
            //切换行为
            timelineComponent.Reload(currentOrder);
        }
    }

    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorInfo))]
    [FriendOf(typeof(InputWait))]    //加特林取消窗口，招式之间相互取消
    public class GCWindow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "GCWindow";
        }

        //OpenGCWindow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBInputHelper.OpenWindow(parser, token, BBTimerInvokeType.GCWindowTimer);
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}