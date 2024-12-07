using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GravityCheckTimer)]
    public class GravityCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            b2Body body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            
            //根运动，不需要模拟重力
            if (body.GetComponent<RootMotionComponent>() != null)
            {
                return;
            }
            
            //将时间戳转成秒单位
            float tick = bbTimer.GetFrameLength() / 10000000f;
            
            //y轴方向当前帧速度改变量
            float g = - timelineComponent.GetParam<long>("Gravity") / 1000f;
            float dv = tick * g;

            Vector2 curV = body.GetVelocity() + new Vector2(0, dv);
            float maxFall = - timelineComponent.GetParam<long>("MaxFall") / 1000f;
            //约束最大下落速度
            if (curV.Y < maxFall)
            {
                curV = new Vector2(curV.X, maxFall);
            }
            
            body.SetVelocity(curV);
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
                DialogueHelper.ScripMatchError(data.opLine);
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