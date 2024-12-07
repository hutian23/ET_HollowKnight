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
            b2Body b2Body = b2GameManager.Instance.GetBody(self.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            
            FlipState preFlip = b2Body.Flip;
            FlipState curFlip = preFlip;
            
            bool left = self.IsPressed(BBOperaType.LEFT) || self.IsPressed(BBOperaType.DOWNLEFT) || self.IsPressed(BBOperaType.UPLEFT);
            bool right = self.IsPressed(BBOperaType.RIGHT) || self.IsPressed(BBOperaType.DOWNRIGHT) || self.IsPressed(BBOperaType.UPRIGHT);
            if (left)
            {
                curFlip = FlipState.Left;
            }
            else if(right)
            {
                curFlip = FlipState.Right;
            }
            
            //转向发生更新
            if (curFlip == preFlip)
            {
                return;
            }
            
            EventSystem.Instance.Invoke(new UpdateFlipCallback(){instanceId = b2Body.unitId,curFlip = curFlip});
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