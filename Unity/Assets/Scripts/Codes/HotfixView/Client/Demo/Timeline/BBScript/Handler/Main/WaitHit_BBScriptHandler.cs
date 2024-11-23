using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.PostStepTimer)]
    [FriendOf(typeof(HitboxComponent))]
    public class WaitHitTimer : BBTimer<HitboxComponent>
    {
        protected override void Run(HitboxComponent hitboxComponent)
        {
            int count = hitboxComponent.CollisionBuffer.Count;
            while (count-- > 0)
            {
                CollisionInfo info = hitboxComponent.CollisionBuffer.Dequeue();
                hitboxComponent.CollisionBuffer.Enqueue(info);
                
                BoxInfo boxInfoA = info.dataA.UserData as BoxInfo;
                BoxInfo boxInfoB = info.dataB.UserData as BoxInfo;
                if (boxInfoA.hitboxType is HitboxType.Hit && boxInfoB.hitboxType is HitboxType.Hurt)
                {
                    Log.Warning("Hit");
                }
            }
        }
    }

    [FriendOf(typeof(BBParser))]
    public class WaitHit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "WaitHit";
        }

        //WaitHit: 
        //  LogWarning: 'Hello_World';
        //  EndHit:
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
            
            long timer = b2GameManager.Instance.GetPostStepTimer().NewFrameTimer(BBTimerInvokeType.PostStepTimer, hitboxComponent);
            token.Add(() =>
            {
                b2GameManager.Instance.GetPostStepTimer().Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}