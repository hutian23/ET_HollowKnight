using System.Text;
using ET.Event;
using Timeline;

namespace ET.Client
{
    [Invoke(TriggerStayType.CollisionEvent)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(TriggerEvent))]
    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(BBParser))]
    public class TriggerStay_HandleCollisionEvent : AInvokeHandler<TriggerStayCallback>
    {
        public override void Handle(TriggerStayCallback args)
        {
            b2Body b2Body = Root.Instance.Get(args.dataA.InstanceId) as b2Body;
            Unit unit = Root.Instance.Get(b2Body.unitId) as Unit;
            TimelineComponent timelineComponent = unit.GetComponent<TimelineComponent>();
            HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();

            //1. find trigger event
            BoxInfo info = args.dataA.UserData as BoxInfo;
            if (!hitboxComponent.ContainTriggerEvent(info.boxName)) return;

            TriggerEvent triggerEvent = hitboxComponent.GetTriggerEvent(info.boxName);
            HandleTriggerEventAsync(triggerEvent).Coroutine();

        }

        private async ETTask HandleTriggerEventAsync(TriggerEvent triggerEvent)
        {
            //拼接字符串
            StringBuilder script = new();
            foreach (string opLine in triggerEvent.opLines)
            {
                script.AppendLine(opLine);
            }
            string str = script.ToString();
            
            //2. BBParser处理碰撞事件
            HitboxComponent hitboxComponent = triggerEvent.GetParent<HitboxComponent>();
            BBParser parser = hitboxComponent.AddChild<BBParser>();
            hitboxComponent.parserIds.Add(parser.Id);
            
            //3. 执行碰撞事件
            parser.InitScript(str);
            await parser.EventInvoke(parser.cancellationToken);
            if(parser.cancellationToken.IsCancel()) return;
            
            //4. 销毁BBParser组件
            parser.Cancel();
            hitboxComponent.parserIds.Remove(parser.Id);
            hitboxComponent.RemoveChild(parser.Id);
            
            await ETTask.CompletedTask;
        }
    }
}