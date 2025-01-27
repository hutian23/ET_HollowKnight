using Box2DSharp.Dynamics;

namespace ET.Client
{
    public class Root_VC_Init_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Init";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            
            //1. Gizmos
           b2WorldManager.Instance.CreateBody(unit.InstanceId, new BodyDef() { BodyType = BodyType.StaticBody });
            
            //2. 启动相机跟随
            
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}