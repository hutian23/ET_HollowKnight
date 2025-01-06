using System.Text.RegularExpressions;

namespace ET.Client
{
    public class DashRecharge_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DashRecharge"; 
        }

        //DashRecharge: 30;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"DashRecharge: (?<WaitFrame>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!int.TryParse(match.Groups["WaitFrame"].Value, out int waitFrame))
            {
                Log.Error($"cannot parse waitFrame to int");
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            ETCancellationToken rechargeToken = timelineComponent.RegistParam("DashRechargeToken", new ETCancellationToken());
            RechargeCor(timelineComponent,waitFrame, rechargeToken).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask RechargeCor(TimelineComponent timelineComponent, int frame, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            await bbTimer.WaitAsync(frame, token);
            if (token.IsCancel())
            {
                timelineComponent.TryRemoveParam("DashRechargeToken");
                return;
            }

            int maxDash = (int)timelineComponent.GetParam<long>("MaxDash");
            timelineComponent.UpdateParam("DashCount", maxDash);
            
            token?.Cancel();
            timelineComponent.TryRemoveParam("DashRechargeToken");
        }
    }
}