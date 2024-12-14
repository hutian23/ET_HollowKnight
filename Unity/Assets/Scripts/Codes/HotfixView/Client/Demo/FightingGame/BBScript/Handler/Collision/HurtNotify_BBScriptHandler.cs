using System.Collections.Generic;
using System.Text.RegularExpressions;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.HurtNotifyTimer)]
    [FriendOf(typeof(b2Unit))]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BBParser))]
    //PostStep生命周期执行
    public class HurtNotifyTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            TimelineComponent timelineComponent = self.GetParent<TimelineComponent>();
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();

            Queue<CollisionInfo> infoQueue = b2Unit.CollisionBuffer;
            int count = infoQueue.Count;

            //缓冲已经调用过受击回调的unit
            HashSetComponent<long> buffSet = self.GetParam<HashSetComponent<long>>("HurtNotify_BuffSet");
            string checkType = self.GetParam<string>("HurtNotify_CheckType");

            while (count-- > 0)
            {
                CollisionInfo info = infoQueue.Dequeue();
                infoQueue.Enqueue(info);

                BoxInfo infoA = info.dataA.UserData as BoxInfo;
                BoxInfo infoB = info.dataB.UserData as BoxInfo;
                if (infoA.hitboxType is not HitboxType.Hit || infoB.hitboxType is not HitboxType.Hurt || info.dataB.InstanceId == 0) continue;

                //根据instanceId找到对应组件
                b2Body b2Body = Root.Instance.Get(info.dataB.InstanceId) as b2Body;
                Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;

                //已经触发过受击回调，是否还要再次调用?
                if (buffSet.Contains(unit.InstanceId) && checkType.Equals("Once")) continue;
                buffSet.Add(unit.InstanceId);

                //调用受击回调
                int startIndex = self.GetParam<int>("HurtNotify_StartIndex");
                int endIndex = self.GetParam<int>("HurtNotify_EndIndex");
                //注册变量供代码块使用
                self.RegistParam("Hurt_CollisionInfo", info);
                self.RegistSubCoroutine(startIndex, endIndex, self.CancellationToken).Coroutine();
                self.TryRemoveParam("Hurt_CollisionInfo");
            }
        }
    }

    [FriendOf(typeof(BBParser))]
    public class HurtNotify_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HurtNotify";
        }

        //Once: 在判定框持续持续窗口内，对于同一unit只会产生1hit
        //Repeat: 在判定框持续窗口内，对于同一unit每间隔一frame产生1hit
        //HurtNotify: Once;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HurtNotify: (?<CheckType>\w+)");
            if (!match.Success)
            {
                DialogueHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //1. 初始化
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            if (parser.ContainParam("HurtNotifyCheckTimer"))
            {
                long preTimer = parser.GetParam<long>("HurtNotifyCheckTimer");
                postStepTimer.Remove(ref preTimer);
            }
            if (parser.ContainParam("HurtNotify_BuffSet"))
            {
                //对象池回收
                HashSetComponent<long> buffSet = parser.GetParam<HashSetComponent<long>>("HurtNotify_BuffSet");
                buffSet.Dispose();
            }
            parser.TryRemoveParam("HurtNotifyCheckTimer");
            parser.TryRemoveParam("HurtNotify_StartIndex");
            parser.TryRemoveParam("HurtNotify_EndIndex");
            parser.TryRemoveParam("HurtNotify_CheckType");
            parser.TryRemoveParam("HurtNotify_BuffSet");
            
            //2. 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.opDict.Count)
            {
                string opline = parser.opDict[index];
                if (opline.Equals("EndNotify:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = index;
            
            //3. 
            long timer = postStepTimer.NewFrameTimer(BBTimerInvokeType.HurtNotifyTimer, parser);
            HashSetComponent<long> _buffSet = HashSetComponent<long>.Create();
            parser.RegistParam("HurtNotifyCheckTimer", timer);
            parser.RegistParam("HurtNotify_StartIndex", startIndex);
            parser.RegistParam("HurtNotify_EndIndex", endIndex);
            parser.RegistParam("HurtNotify_CheckType", match.Groups["CheckType"].Value);
            parser.RegistParam("HurtNotify_BuffSet", _buffSet);
            
            //4. 退出行为协程，初始化
            token.Add(() =>
            {
                postStepTimer.Remove(ref timer);
                _buffSet.Dispose();
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}