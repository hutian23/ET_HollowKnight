using System.Collections.Generic;
using Timeline;

namespace ET.Client
{
    [ComponentOf(typeof (TimelineComponent))]
    public class HitboxComponent: Entity, IAwake, IDestroy
    {
        public HitboxKeyframe keyFrame;

        public List<long> triggerEventIds = new();

        //Hitbox Name ---> Trigger Event
        public Dictionary<string, long> HitboxDict = new();
    }
}