using System.Collections.Generic;
using System.Text.RegularExpressions;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(BBTimerInvokeType.HurtNotifyTimer)]
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(BBParser))]
    //PostStep生命周期执行
    public class HitNotifyTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            B2Unit b2Unit = self.GetParent<Unit>().GetComponent<B2Unit>();

            Queue<CollisionInfo> infoQueue = b2Unit.CollisionBuffer;
            int count = infoQueue.Count;

            //缓冲已经调用过受击回调的unit
            HashSetComponent<long> buffSet = self.GetParam<HashSetComponent<long>>("HitNotify_BuffSet");
            string checkType = self.GetParam<string>("HitNotify_CheckType");

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
                int startIndex = self.GetParam<int>("HitNotify_StartIndex");
                int endIndex = self.GetParam<int>("HitNotify_EndIndex");
                
                //注册变量供代码块使用
                self.RegistParam("HitNotify_CollisionInfo", info);
                self.RegistSubCoroutine(startIndex, endIndex, self.CancellationToken).Coroutine();
                self.TryRemoveParam("HitNotify_CollisionInfo");
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

        //Once: 在判定框持续持续窗口内，对于同一unit只会产生1hit
        //Repeat: 在判定框持续窗口内，对于同一unit每间隔一frame产生1hit
        //HurtNotify: Once;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Match match = Regex.Match(data.opLine, @"HitNotify: (?<CheckType>\w+)");
            if (!match.Success)
            {
                ScriptHelper.ScripMatchError(data.opLine);
                return Status.Failed;
            }
            
            //1. 初始化
            BBTimerComponent postStepTimer = b2WorldManager.Instance.GetPostStepTimer();
            if (parser.ContainParam("HitNotifyCheckTimer"))
            {
                long preTimer = parser.GetParam<long>("HitNotifyCheckTimer");
                postStepTimer.Remove(ref preTimer);
            }
            if (parser.ContainParam("HitNotify_BuffSet"))
            {
                //对象池回收
                HashSetComponent<long> buffSet = parser.GetParam<HashSetComponent<long>>("HitNotify_BuffSet");
                buffSet.Dispose();
            }
            parser.TryRemoveParam("HitNotifyCheckTimer");
            parser.TryRemoveParam("HitNotify_StartIndex");
            parser.TryRemoveParam("HitNotify_EndIndex");
            parser.TryRemoveParam("HitNotify_CheckType");
            parser.TryRemoveParam("HitNotify_BuffSet");
            
            //2. 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.OpDict.Count)
            {
                string opline = parser.OpDict[index];
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
            parser.RegistParam("HitNotifyCheckTimer", timer);
            parser.RegistParam("HitNotify_StartIndex", startIndex);
            parser.RegistParam("HitNotify_EndIndex", endIndex);
            parser.RegistParam("HitNotify_CheckType", match.Groups["CheckType"].Value);
            parser.RegistParam("HitNotify_BuffSet", _buffSet);
            
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