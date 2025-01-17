using System.Text.RegularExpressions;

namespace ET.Client
{
    public class IdleAnim_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "IdleAnim";
        }

        //IdleAnim: Rg_IdleAnim;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, "IdleAnim: (?<Animation>.*?);");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            IdleAnimCor(parser.GetParent<Unit>(), match.Groups["Animation"].Value, token).Coroutine();
            
            await ETTask.CompletedTask;
            return Status.Success;
        }

        private async ETTask IdleAnimCor(Unit unit, string behaviorName, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();
            
            await bbTimer.WaitAsync(300, token);
            if (token.IsCancel()) return;
            machine.Reload(behaviorName);
        }
    }
}