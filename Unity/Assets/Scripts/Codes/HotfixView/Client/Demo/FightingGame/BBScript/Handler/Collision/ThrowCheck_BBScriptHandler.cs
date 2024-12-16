using System.Collections.Generic;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.ThrowCheckTimer)]
    [FriendOf(typeof(b2Unit))]
    public class ThrowCheckTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();
            
            Queue<CollisionInfo> infoQueue = b2Unit.CollisionBuffer;
            int count = infoQueue.Count;

            while (count-- > 0)
            {
                CollisionInfo info = infoQueue.Dequeue();
                infoQueue.Enqueue(info);

                BoxInfo infoA = info.dataA.UserData as BoxInfo;
                BoxInfo infoB = info.dataB.UserData as BoxInfo;
                if (infoA.hitboxType is HitboxType.Throw &&
                    infoB.hitboxType is HitboxType.Hurt)
                {
                    //here: 注册变量供后面脚本使用
                    self.RegistParam("ThrowHurt", infoB);
                    
                    //销毁定时器
                    long timer = self.GetParam<long>("ThrowCheckTimer");
                    postStepTimer.Remove(ref timer);
                    self.TryRemoveParam("ThrowCheckTimer");
                    break;
                }
            }
        }
    }

    public class ThrowCheck_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ThrowCheck";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            
            // 初始化
            if (parser.ContainParam("ThrowCheckTimer"))
            {
                long preTimer = parser.GetParam<long>("ThrowCheckTimer");
                postStepTimer.Remove(ref preTimer);
            }
            parser.TryRemoveParam("ThrowCheckTimer");
            
            // 注册定时器
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.ThrowCheckTimer, parser);
            parser.RegistParam("ThrowCheckTimer", timer);
            token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}