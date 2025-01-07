using System;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AccelerationXTimer)]
    public class AccelerationXTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();
            
            float ax = self.GetParam<long>("AccelerationX") / 1000f;
            float maxV = self.GetParam<long>("MaxV") / 1000f;
            float dv = (1 / 60f) * ax;
            
            float curV = b2Unit.GetVelocity().X + dv;
            //限制速度大小
            if (Math.Abs(curV) >= maxV)
            {
                curV = Math.Sign(curV) * maxV;
            }
            b2Unit.SetVelocityX(curV);
        }
    }
    
    public class AccelerationX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AccelerationX";
        }

        //AccelerationX: a, MaxV(矢量);
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "AccelerationX: (?<A>.*?), (?<MaxV>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["A"].Value, out long accelerationX) || !long.TryParse(match.Groups["MaxV"].Value, out long maxV))
            {
                Log.Error($"cannot format {match.Groups["A"].Value} / {match.Groups["MaxV"].Value} to long!!!");
                return Status.Failed;
            }
            
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            //初始化
            if (parser.ContainParam("AccelerationXTimer"))
            {
                long timer = parser.GetParam<long>("AccelerationXTimer");
                bbTimer.Remove(ref timer);
            }
            parser.TryRemoveParam("AccelerationXTimer");
            parser.TryRemoveParam("AccelerationX");
            parser.TryRemoveParam("MaxV");
            
            parser.RegistParam("AccelerationX", accelerationX);
            parser.RegistParam("MaxV", maxV);
            long curTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.AccelerationXTimer, parser);
            parser.RegistParam("AccelerationXTimer", curTimer);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}