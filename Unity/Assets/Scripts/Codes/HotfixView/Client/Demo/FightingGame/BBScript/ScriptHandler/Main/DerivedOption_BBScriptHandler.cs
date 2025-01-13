namespace ET.Client
{
    public class DerivedOption_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "DerivedOption";
        }

        //DerivedOption: 5LPPressed, 'Rg_AirDashAttack';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}