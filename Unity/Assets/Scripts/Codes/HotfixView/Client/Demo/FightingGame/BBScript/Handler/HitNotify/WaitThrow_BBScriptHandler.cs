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
                if (boxInfoA.hitboxType is HitboxType.Throw && boxInfoB.hitboxType is HitboxType.Squash)
                {
                    //移除定时器
                    long timer = self.GetParam<long>("ThrowCheckTimer");
                    postStepTimer.Remove(ref timer);
                    self.RemoveParam("ThrowCheckTimer");
                    self.RegistParam("ThrowHit", info.dataB);
                    break;
                }
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