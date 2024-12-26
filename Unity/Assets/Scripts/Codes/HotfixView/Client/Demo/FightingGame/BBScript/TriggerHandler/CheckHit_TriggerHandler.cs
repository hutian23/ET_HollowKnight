using ET.Event;
using Timeline;

namespace ET.Client
{
    [FriendOf(typeof(b2Unit))]
    public class CheckHit_TriggerHandler : BBTriggerHandler
    {
        public override string GetTriggerType()
        {
            return "Hit";
        }

        //该trigger只能在 PostStep生命周期中使用
        //RegistCallback: (Hit: xxx), 'HitCheck'
        public override bool Check(BBParser parser, BBScriptData data)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            b2Unit b2Unit = timelineComponent.GetComponent<b2Unit>();
           
            int count = b2Unit.CollisionBuffer.Count;
            while (count-- > 0)
            {
                CollisionInfo info = b2Unit.CollisionBuffer.Dequeue();
                b2Unit.CollisionBuffer.Enqueue(info);
                
                BoxInfo boxInfoA = info.dataA.UserData as BoxInfo;
                BoxInfo boxInfoB = info.dataB.UserData as BoxInfo;
                if (boxInfoA.hitboxType is HitboxType.Hit && boxInfoB.hitboxType is HitboxType.Hurt)
                {
                    return true;
                }
            }
            return false;
        }
    }
}