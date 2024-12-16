namespace ET.Client
{
    [Invoke(BBTimerInvokeType.UpdateFlipTimer)]
    [FriendOf(typeof(InputWait))]
    [FriendOf(typeof(b2Body))]
    public class UpdateFlipTimer : BBTimer<InputWait>
    {
        protected override void Run(InputWait self)
        {
            //更新刚体朝向
            b2Body b2Body = b2WorldManager.Instance.GetBody(self.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            if (self.IsPressing(BBOperaType.LEFT) ||
                self.IsPressing(BBOperaType.DOWNLEFT) ||
                self.IsPressing(BBOperaType.UPLEFT))
            {
                b2Body.SetFlip(FlipState.Left);
            }
            else if(self.IsPressing(BBOperaType.RIGHT) || 
                    self.IsPressing(BBOperaType.DOWNRIGHT) ||
                    self.IsPressing(BBOperaType.UPRIGHT))
            {
                b2Body.SetFlip(FlipState.Right);
            }
        }
    }

    public class UpdateFlip_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "UpdateFlip";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.UpdateFlipTimer, inputWait);
            token.Add(() => { bbTimer.Remove(ref timer); });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}