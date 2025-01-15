namespace ET.Client
{
    [FriendOf(typeof (BehaviorInfo))]
    public static class BehaviorInfoSystem
    {
        public class SkillInfoDestroySystem: DestroySystem<BehaviorInfo>
        {
            protected override void Destroy(BehaviorInfo self)
            {
                self.behaviorName = string.Empty;
                self.behaviorOrder = 0;
                self.moveType = MoveType.None;
            }
        }
    }
}