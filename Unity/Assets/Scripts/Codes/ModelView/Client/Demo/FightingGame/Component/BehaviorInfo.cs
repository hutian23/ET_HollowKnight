namespace ET.Client
{
    [ChildOf(typeof (BehaviorMachine))]
    public class BehaviorInfo: Entity, IAwake, IDestroy
    {
        public string behaviorName;
        public int behaviorOrder;
        public MoveType moveType;
    }

    public enum MoveType
    {
        None = 0,
        Transition = 1,
        Move = 2,
        Normal = 3,
        Special = 4,
        Super = 5,
        HitStun = 6,
        Etc = 7
    }
}