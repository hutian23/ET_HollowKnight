using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    public class Thrones_Goto_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Thrones_Goto";
        }

        //Thrones_Goto: Thrones_WallThrow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"Thrones_Goto: (?<State>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            parser.Invoke(parser.GetFunctionPointer(match.Groups["State"].Value, "Main"), parser.CancellationToken).Coroutine();

            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}