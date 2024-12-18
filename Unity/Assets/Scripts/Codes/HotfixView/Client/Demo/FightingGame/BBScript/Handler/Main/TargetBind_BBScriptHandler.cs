using System.Numerics;
using System.Text.RegularExpressions;

namespace ET.Client
{
    public class TargetBind_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "TargetBind";
        }

        //TargetBind: -2000, 1000;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "TargetBind: (?<bindX>.*?), (?<bindY>.*?);");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            if (!long.TryParse(match.Groups["bindX"].Value, out long bindX) || !long.TryParse(match.Groups["bindY"].Value, out long bindY))
            {
                Log.Error($"cannot format bindX/bindY to long");
                return Status.Failed;
            }

            if (!parser.ContainParam("TargetBind"))
            {
                Log.Error($"does not exist targetBind unit!!");
                return Status.Failed;
            }
            
            long bodyId = parser.GetParam<long>("TargetBind");
            b2Body bodyA = b2WorldManager.Instance.GetBody(parser.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            b2Body bodyB = Root.Instance.Get(bodyId) as b2Body;

            // 翻转
            Vector2 pos = bodyA.GetPosition();
            Vector2 targetBindPos = pos + new Vector2(bindX * -bodyA.GetFlip(), bindY) / 1000f;
            bodyB.SetPosition(targetBindPos);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}