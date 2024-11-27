using System.Numerics;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ThrowCheckTimer)]
    [FriendOf(typeof(HitboxComponent))]
    public class ThrowCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            BBTimerComponent postStepTimer = b2GameManager.Instance.GetPostStepTimer();
            
            int count = hitboxComponent.CollisionBuffer.Count;
            while (count -- > 0)
            {
                CollisionInfo info = hitboxComponent.CollisionBuffer.Dequeue();
                hitboxComponent.CollisionBuffer.Enqueue(info);
                
                BoxInfo boxInfoA = info.dataA.UserData as BoxInfo;
                BoxInfo boxInfoB = info.dataB.UserData as BoxInfo;
                if (boxInfoA.hitboxType is not HitboxType.Throw || boxInfoB.hitboxType is not HitboxType.Squash) continue;
                
                //1. 移除定时器
                long timer = self.GetParam<long>("ThrowCheckTimer");
                postStepTimer.Remove(ref timer);
                self.RemoveParam("ThrowCheckTimer");
                    
                //2. 
                self.RegistParam("ThrowHit", info.dataB);
                    
                //3. Set targetBind 
                Log.Warning("Throw");
                Vector2 targetBind = self.GetParam<Vector2>("TargetBind");
                b2Body b2Body = b2GameManager.Instance.GetBody(timelineComponent.GetParent<Unit>().InstanceId);
                b2Body hitBody = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
                hitBody.SetPosition(b2Body.GetPosition() + targetBind);
                
                break;
            }
        }
    }

    public class WaitThrow_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WaitThrow";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            BBTimerComponent postStepTimer = b2GameManager.Instance.GetPostStepTimer();

            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.ThrowCheckTimer, bbParser);
            bbParser.RegistParam("ThrowCheckTimer", timer);
            
            token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}