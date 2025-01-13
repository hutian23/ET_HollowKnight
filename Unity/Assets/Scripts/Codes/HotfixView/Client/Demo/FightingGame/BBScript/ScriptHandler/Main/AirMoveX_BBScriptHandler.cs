using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AirMoveTimer)]
    public class AirMoveTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            InputWait inputWait = timelineComponent.GetComponent<InputWait>();
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();
            
            //输入左右相关的指令才会生效水平移动的效果
            bool direction = inputWait.IsPressing(BBOperaType.MIDDLE) || inputWait.IsPressing(BBOperaType.UP) || inputWait.IsPressing(BBOperaType.DOWN);
            
            //当前回中，则不会进行移动
            if (self.GetParam<bool>("InertiaEffect") && direction) return;
            self.UpdateParam("InertiaEffect", false);
            
            b2Unit.SetVelocityX(direction ? 0 : self.GetParam<long>("AirMoveX") / 1000f);
        }
    }

    public class AirMoveX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AirMoveX";
        }

        //AirMoveX: (水平移动速度。标量)
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AirMoveX: ((?<MoveX>\w+))");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["MoveX"].Value, out long moveX))
            {
                Log.Error($"cannot format {match.Groups["MoveX"].Value} to long");
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            //初始化
            parser.TryRemoveParam("InertiaEffect");
            parser.TryRemoveParam("AirMoveX");
            if (parser.ContainParam("AirMoveXTimer"))
            {
                long preTimer = parser.GetParam<long>("AirMoveXTimer");
                bbTimer.Remove(ref preTimer);
            }
            parser.TryRemoveParam("AirMoveXTimer");
            
            //注册变量
            parser.RegistParam("InertiaEffect", true);
            parser.RegistParam("AirMoveX", moveX);
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.AirMoveTimer, parser);
            parser.RegistParam("AirMoveXTimer", timer);
            token.Add(() => { bbTimer.Remove(ref timer);});
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}