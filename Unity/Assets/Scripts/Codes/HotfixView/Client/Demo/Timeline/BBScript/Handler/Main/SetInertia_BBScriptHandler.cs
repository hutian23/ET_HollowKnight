using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.InertiaTimer)]
    public class InertiaTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            b2Body b2Body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            int preFlip = self.GetParam<int>("Inertia_PreFlip");
            int curFlip = b2Body.GetFlip();
            long timer = self.GetParam<long>("Inertia_Timer");
            
            if (curFlip == preFlip)
            {
                b2Body.SetVelocityX(self.GetParam<long>("Inertia") / 1000f);
                return;
            }
            
            bbTimer.Remove(ref timer);
            self.RemoveParam("Inertia");
            self.RemoveParam("Inertia_PreFlip");
            self.RemoveParam("Inertia_Timer");
        }
    }
            
    public class SetInertia_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "SetInertia";
        }

        //Inertia: 30;          
        //说明: 为了动作的连贯性， AirDash行为切换到AirBone行为时，需要保留一个水平速度，不然就会看到玩家直直地掉下来
        //如果速度方向和转向相同，则保持这个水平速度，如果转向改变，不再保持这个速度
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "Inertia: (?<Inertia>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["Inertia"].Value, out long inertia))
            {
                Log.Error($"cannot parser {match.Groups["Inertia"].Value} to long");
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = parser.GetParent<TimelineComponent>().GetComponent<BBTimerComponent>();
            b2Body b2Body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.InertiaTimer, parser);
            parser.RegistParam("Inertia_Timer", timer);
            parser.RegistParam("Inertia",inertia);
            parser.RegistParam("Inertia_PreFlip", b2Body.GetFlip());
            
            token.Add(() =>
            {
                bbTimer.Remove(ref timer); 
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}