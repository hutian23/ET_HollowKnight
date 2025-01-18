using System.Text.RegularExpressions;
using Timeline;

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
            b2Body b2Body = b2WorldManager.Instance.GetBody(self.GetParent<Unit>().InstanceId);
            if (self.IsPressing(BBOperaType.LEFT) ||
                self.IsPressing(BBOperaType.DOWNLEFT) ||
                self.IsPressing(BBOperaType.UPLEFT))
            {
                b2Body.SetFlip(FlipState.Left, true);
            }
            else if(self.IsPressing(BBOperaType.RIGHT) || 
                    self.IsPressing(BBOperaType.DOWNRIGHT) ||
                    self.IsPressing(BBOperaType.UPRIGHT))
            {
                b2Body.SetFlip(FlipState.Right, true);
            }
        }
    }

    public class UpdateFlip_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "UpdateFlip";
        }
        
        //UpdateFlip: Once;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"UpdateFlip: (?<Flip>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            Unit unit = parser.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            InputWait inputWait = unit.GetComponent<InputWait>();
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);

            switch (match.Groups["Flip"].Value)
            {
                case "Once":
                {
                    //这里为什么不采用OnceTimer? OnceTimer会在下一帧更新，这里希望是立刻更新
                    if (inputWait.IsPressing(BBOperaType.LEFT) ||
                        inputWait.IsPressing(BBOperaType.UPLEFT) ||
                        inputWait.IsPressing(BBOperaType.DOWNLEFT))
                    {
                        b2Body.SetFlip(FlipState.Left);
                    }
                    else if (inputWait.IsPressing(BBOperaType.RIGHT) ||
                             inputWait.IsPressing(BBOperaType.UPRIGHT) ||
                             inputWait.IsPressing(BBOperaType.DOWNRIGHT))
                    {
                        b2Body.SetFlip(FlipState.Right);
                    }
                    return Status.Success;
                }
                case "Repeat":
                {
                    long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.UpdateFlipTimer, inputWait);
                    token.Add(() =>
                    {
                        bbTimer.Remove(ref timer);
                    });
                    return Status.Success;
                }
            }
            
            await ETTask.CompletedTask;
            return Status.Failed;
        }
    }
}