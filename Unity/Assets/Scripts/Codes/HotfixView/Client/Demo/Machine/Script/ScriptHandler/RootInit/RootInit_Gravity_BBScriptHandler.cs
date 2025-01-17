using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GravityCheckTimer)]
    public class GravityCheckTimer : BBTimer<Unit>
    {
        protected override void Run(Unit self)
        {
            BehaviorMachine machine = self.GetComponent<BehaviorMachine>();
            B2Unit b2Unit = self.GetComponent<B2Unit>();
            BBNumeric numeric = self.GetComponent<BBNumeric>();
            
            //y轴方向当前帧速度改变量
            float g = - machine.GetParam<long>("Gravity") / 10000f;
            //定时器对TimeScale更改无感知，正常按照60帧执行逻辑
            float dv = (1 / 60f) * g;
            //约束最大下落速度
            float curY = b2Unit.GetVelocity().Y + dv;
            float maxFall = numeric.GetAsFloat("MaxFall");
            b2Unit.SetVelocityY(curY < maxFall? maxFall : curY);   
        }
    }
    [FriendOf(typeof(BehaviorMachine))]
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
            if (!long.TryParse(match.Groups["gravity"].Value, out long gravity))
            {
                Log.Error($"cannot format {match.Groups["gravity"].Value} to long!!!");
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();

            //1. 初始化
            machine.TryRemoveParam("Gravity");
            if (machine.ContainParam("GravityTimer"))
            {
                long timer = machine.GetParam<long>("GravityTimer");
                bbTimer.Remove(ref timer);
                machine.RemoveParam("GravityTimer");
            }

            //2. 注册重力变量
            long gravityTimer = bbTimer.NewFrameTimer(BBTimerInvokeType.GravityCheckTimer, unit);
            machine.RegistParam("Gravity", gravity);
            machine.RegistParam("GravityTimer", gravityTimer);

            //3. 热更新 or 销毁unit 时销毁定时器
            machine.Token.Add(() =>
            {
                if (machine.ContainParam("GravityTimer"))
                {
                    long _timer = machine.GetParam<long>("GravityTimer");
                    bbTimer.Remove(ref _timer);
                    machine.TryRemoveParam("GravityTimer");
                }
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}