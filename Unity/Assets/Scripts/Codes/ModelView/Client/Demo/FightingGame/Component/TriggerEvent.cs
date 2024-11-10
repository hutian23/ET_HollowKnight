using System.Collections.Generic;

namespace ET.Client
{
    [ChildOf(typeof(HitboxComponent))]
    public class TriggerEvent : Entity,IAwake,IDestroy
    {
        public TriggerType TriggerType;

        public List<string> opLines = new();
    }

    public enum TriggerType
    {
        None = 0,
        TriggerEnter = 1,
        TriggerStay = 2,
        TriggerExit = 3
    }
}