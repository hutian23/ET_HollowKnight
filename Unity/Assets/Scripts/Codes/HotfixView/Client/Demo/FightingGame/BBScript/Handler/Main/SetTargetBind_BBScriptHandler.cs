using System.Text.RegularExpressions;

namespace ET.Client
{
    public class SetTargetBind_BBScriptHandler : BBScriptHandler
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
            
            parser.TryRemoveParam("TargetBind");
            parser.RegistParam("TargetBind",new System.Numerics.Vector2(bindX / 1000f, bindY / 1000f));
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}