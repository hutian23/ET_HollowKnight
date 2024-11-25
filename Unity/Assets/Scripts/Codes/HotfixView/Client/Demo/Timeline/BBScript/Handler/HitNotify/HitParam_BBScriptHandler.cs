using System.Text.RegularExpressions;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class HitParam_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitParam";
        }

        //HitParam: StopFrame, 10;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "HitParam: (?<ParamName>.*?), (?<ParamValue>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            string paramName = match.Groups["ParamName"].Value;
            if (!long.TryParse(match.Groups["ParamValue"].Value, out long paramValue))
            {
                Log.Error($"cannot format {match.Groups["ParamValue"].Value} to long!");
                return Status.Failed;
            }

            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            FixtureData dataB = bbParser.GetParam<FixtureData>("HitData");
            b2Body b2Body = Root.Instance.Get(dataB.InstanceId) as b2Body;
            
            //注册变量
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            unit.GetComponent<TimelineComponent>().GetComponent<BehaviorBuffer>().RegistParam(paramName, paramValue);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}