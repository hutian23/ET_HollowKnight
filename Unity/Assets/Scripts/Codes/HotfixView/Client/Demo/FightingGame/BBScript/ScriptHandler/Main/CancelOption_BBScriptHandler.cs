using System.Text.RegularExpressions;

namespace ET.Client
{
    [FriendOf(typeof (BehaviorMachine))]
    [FriendOf(typeof (BehaviorInfo))]
    public class CancelOption_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CancelOption";
        }

        //CancelOption: Rg_Idle;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"CancelOption: (?<Option>\w+);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }

            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            HashSetComponent<string> cancelOptions = machine.GetParam<HashSetComponent<string>>("CancelWindow_Options");
            cancelOptions.Add(match.Groups["Option"].Value);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}