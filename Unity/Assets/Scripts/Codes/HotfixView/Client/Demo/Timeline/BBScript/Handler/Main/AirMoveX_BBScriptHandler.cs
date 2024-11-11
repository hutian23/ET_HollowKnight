using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.AirMoveTimer)]
    public class AirMoveTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            //惯性
            if (self.ContainParam("Inertia"))
            {
                return;
            }
                
            InputWait inputWait = self.GetParent<TimelineComponent>().GetComponent<InputWait>();

            // -1 -- left 1 -- right 0 -- middle
            int flip = 0;
            bool Left = inputWait.IsPressed(BBOperaType.LEFT) | inputWait.IsPressed(BBOperaType.UPLEFT) | inputWait.IsPressed(BBOperaType.DOWNLEFT);
            bool right = inputWait.IsPressed(BBOperaType.RIGHT) | inputWait.IsPressed(BBOperaType.DOWNRIGHT) | inputWait.IsPressed(BBOperaType.UPRIGHT);
            //回中
            if (Left)
            {
                flip = -1;
            }
            else if (right)
            {
                flip = 1;
            }
            
            b2Body B2body = b2GameManager.Instance.GetBody(self.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            Vector2 curV = new(flip * self.GetParam<long>("AirMoveX") / 1000f ,B2body.GetVelocity().Y);
            B2body.SetVelocity(curV);
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
            
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();
            
            long timer = bbTimer.NewFrameTimer(BBTimerInvokeType.AirMoveTimer, bbParser);
            token.Add(() => { bbTimer.Remove(ref timer);});

            bbParser.RegistParam("AirMoveX", moveX);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}