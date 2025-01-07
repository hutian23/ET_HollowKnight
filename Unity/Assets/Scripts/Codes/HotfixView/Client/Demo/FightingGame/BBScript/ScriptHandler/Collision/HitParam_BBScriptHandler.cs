using System.Text.RegularExpressions;
using ET.Event;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitParam_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitParam";
        }

        //代码块中可执行，需要注册Hurt_CollisionInfo变量
        //HitParam: StopFrame, 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitParam: (?<ParamName>.*?), (?<ParamValue>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            CollisionInfo info = parser.GetParam<CollisionInfo>("Hurt_CollisionInfo");
            
            //查询组件
            b2Body b2Body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();
            
            //注册变量
            buffer.RegistParam(match.Groups["ParamName"].Value, match.Groups["ParamValue"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}