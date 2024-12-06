using System.Collections.Generic;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.HitNotifyTimer)]
    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(BBParser))]
    public class PostStepTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();

            //受击者已经触发过回调，不会再次触发
            HashSet<long> hitBuffer = self.GetParam<HashSet<long>>("HitBuffer");

            int count = hitboxComponent.CollisionBuffer.Count;
            while (count-- > 0)
            {
                CollisionInfo info = hitboxComponent.CollisionBuffer.Dequeue();
                hitboxComponent.CollisionBuffer.Enqueue(info);

                BoxInfo boxInfoA = info.dataA.UserData as BoxInfo;
                BoxInfo boxInfoB = info.dataB.UserData as BoxInfo;
                if (hitBuffer.Contains(info.dataB.InstanceId) ||
                    boxInfoA.hitboxType is not HitboxType.Hit ||
                    boxInfoB.hitboxType is not HitboxType.Hurt)
                {
                    continue;
                }

                //执行回调
                int startIndex = self.GetParam<int>("HitNotify_StartIndex");
                int endIndex = self.GetParam<int>("HitNotify_EndIndex");

                self.RegistParam("HitData", info.dataB);
                self.RegistSubCoroutine(startIndex, endIndex, self.cancellationToken).Coroutine();
                self.RemoveParam("HitData");

                hitBuffer.Add(info.dataB.InstanceId);
            }
        }
    }
    [FriendOf(typeof(BBParser))]
    public class HitNotify_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitNotify";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();


            //缓存b2Body.instanceId
            bbParser.RegistParam("HitBuffer", new HashSet<long>());
            long timer = b2GameManager.Instance.GetPostStepTimer().NewFrameTimer(BBTimerInvokeType.HitNotifyTimer, bbParser);
            bbParser.RegistParam("HitNotifyTimer", timer);

            //跳过代码块
            int index = bbParser.function_Pointers[data.functionID];
            int endIndex = index, startIndex = index+1;
            while (++index < bbParser.opDict.Count)
            {
                string opLine = bbParser.opDict[index];
                if (opLine.Equals("EndHitNotify:"))
                {
                    endIndex = index;
                    break;
                }
            }
            bbParser.function_Pointers[data.functionID] = endIndex;
            bbParser.RegistParam("HitNotify_StartIndex", startIndex);
            bbParser.RegistParam("HitNotify_EndIndex", endIndex);
            
            token.Add(() =>
            {
                b2GameManager.Instance.GetPostStepTimer().Remove(ref timer);
            });

            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}