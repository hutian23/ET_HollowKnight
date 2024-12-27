namespace ET.Client
{
    
    public class CreateProjectile_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateProjectile";
        }

        //CreateBall:
        //  Ball_LocalPos: 1000, -2000;
        //  Ball_Type: 
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}