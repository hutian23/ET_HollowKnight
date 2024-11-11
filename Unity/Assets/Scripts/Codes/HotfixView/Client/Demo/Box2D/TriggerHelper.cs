using System.Text;

namespace ET.Client
{
    [FriendOf(typeof(TriggerEvent))]
    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(BBParser))]
    public static class TriggerHelper
    {
        public static async ETTask HandleTriggerEventAsync(TriggerEvent triggerEvent)
        {
            //拼接字符串
            StringBuilder script = new();
            foreach (string opLine in triggerEvent.opLines)
            {
                script.AppendLine(opLine);
            }
            string str = script.ToString();

            //2. 添加BBParser处理碰撞事件
            HitboxComponent hitboxComponent = triggerEvent.GetParent<HitboxComponent>();
            TimelineComponent timelineComponent = hitboxComponent.GetParent<TimelineComponent>();
            BBParser parser = hitboxComponent.AddChild<BBParser>();
            parser.SetEntityId(timelineComponent.InstanceId);
            hitboxComponent.parserIds.Add(parser.Id);

            //3. 执行碰撞事件
            parser.InitScript(str);
            await parser.EventInvoke(parser.cancellationToken);
            if (parser.cancellationToken.IsCancel()) return;

            //4. 销毁BBParser组件
            parser.Cancel();
            hitboxComponent.parserIds.Remove(parser.Id);
            hitboxComponent.RemoveChild(parser.Id);

            await ETTask.CompletedTask;
        }
    }
}