using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GravityCheckTimer)]
    public class GravityCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            if (self.ContainParam("ApplyRootMotion")) return;
            
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();
            
            //y轴方向当前帧速度改变量
            float g = - timelineComponent.GetParam<long>("Gravity") / 1000f;
            //定时器对TimeScale更改无感知，正常按照60帧执行逻辑
            float dv = (1 / 60f) * g;

            float curY = b2Unit.GetVelocity().Y + dv;
            float maxFall = - timelineComponent.GetParam<long>("MaxFall") / 1000f;
            //约束最大下落速度
            if (curY < maxFall)
            {
                curY = maxFall;
            }
            b2Unit.SetVelocityY(curY);   
        }
    }

    public class RootInit_Gravity_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Gravity";
        }

        //Gravity: 40;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Gravity: (?<gravity>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            //注册重力变量
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            
            long.TryParse(match.Groups["gravity"].Value,out long gravity);
            timelineComponent.TryRemoveParam("Gravity");
            timelineComponent.RegistParam("Gravity", gravity);
            
            //启动定时器
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            if (timelineComponent.ContainParam("GravityTimer"))
            {
                long timer = timelineComponent.GetParam<long>("GravityTimer");
                bbTimer.Remove(ref timer);
                timelineComponent.RemoveParam("GravityTimer");
            }

            long gravityTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.GravityCheckTimer, parser);
            timelineComponent.RegistParam("GravityTimer", gravityTimer);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}