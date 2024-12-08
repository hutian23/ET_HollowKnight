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
            
            //当前回中，则不会进行移动
            bool IsMiddle = inputWait.IsPressing(BBOperaType.MIDDLE);
            b2Body B2body = b2WorldManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
            B2body.SetVelocityX(IsMiddle ? 0 : self.GetParam<long>("AirMoveX") / 1000f);
        }
    }

    public class AirMoveX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "AirMoveX";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"AirMoveX: ((?<MoveX>\w+))");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["MoveX"].Value, out long moveX))
            {
                Log.Error($"cannot format {match.Groups["MoveX"].Value} to long");
                return Status.Failed;
            }

            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.AirMoveTimer, parser);
            token.Add(() => { bbTimer.Remove(ref timer);});

            parser.RegistParam("AirMoveX", moveX);
            parser.RegistParam("AirMoveXTimer", timer);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}