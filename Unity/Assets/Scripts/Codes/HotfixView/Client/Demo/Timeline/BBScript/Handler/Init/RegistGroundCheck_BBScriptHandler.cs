using System.Numerics;
using Box2DSharp.Dynamics;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.GroundCheckTimer)]
    [FriendOf(typeof (b2GameManager))]
    public class GroundCheckTimer: BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            SkillBuffer buffer = self.GetParent<TimelineComponent>().GetComponent<SkillBuffer>();
            buffer.TryRemoveParam("OnGround");
            
            b2Body body = b2GameManager.Instance.GetBody(self.GetParent<TimelineComponent>().GetParent<Unit>().InstanceId);
            World world = b2GameManager.Instance.B2World.World;
            
            //RayCast callback
            GroundCheckRayCastCallback callback = GroundCheckRayCastCallback.Create();
            world.RayCast(callback, body.GetPosition(), body.GetPosition() + new Vector2(0, -2.6f));
            
            //变量注册到SkillBuffer中，注意切换行为时，变量会全部销毁
            bool OnGround = callback.Hit;
            buffer.RegistParam("OnGround", OnGround);
            
            Log.Warning(OnGround.ToString());
            
            //回收callback
            callback.Recycle();            
        }
    }

    public class RegistGroundCheck_BBScriptHandler: BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistGroundCheck";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            bbTimer.NewFrameTimer(BBTimerInvokeType.GroundCheckTimer, parser);

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}