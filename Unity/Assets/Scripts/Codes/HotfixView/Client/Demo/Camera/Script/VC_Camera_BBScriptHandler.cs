using System.Text.RegularExpressions;

namespace ET.Client
{
    public class VC_Camera_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Camera";
        }

        //VC_Camera: DefaultCamera;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"VC_Camera: (?<Name>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            parser.Invoke(parser.GetFunctionPointer(match.Groups["Name"].Value, "Main"), token).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}