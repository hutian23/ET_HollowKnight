using System.Text.RegularExpressions;

namespace ET.Client
{
    public class BallParam_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "BallParam";
        }

        //BallParam: PosX, 1000;
        //BallParam: VelX, 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine,@"BallParam: (?<BallType>\w+), (?<BallParam>[-\w]+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            long instanceId = parser.GetParam<long>("BallId");
            Unit ball = Root.Instance.Get(instanceId) as Unit;
            TimelineComponent timelineComponent = ball.GetComponent<TimelineComponent>();
 
            //注册变量
            timelineComponent.RegistParam(match.Groups["BallType"].Value, match.Groups["BallParam"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}