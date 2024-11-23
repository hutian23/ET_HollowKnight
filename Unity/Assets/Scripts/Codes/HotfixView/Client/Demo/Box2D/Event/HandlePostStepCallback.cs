namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(TimelineManager))]
    [FriendOf(typeof(HitboxComponent))]
    [FriendOf(typeof(b2GameManager))]
    public class HandlePostStepCallback : AInvokeHandler<PostStepCallback>
    {
        public override void Handle(PostStepCallback args)
        {
            b2GameManager.Instance.GetPostStepTimer().Step();
            
            b2GameManager.Instance.B2World.PostStepCallback?.Invoke();
            b2GameManager.Instance.B2World.PostStepCallback = null;

            //清空碰撞缓冲区
            foreach (long instanceId in TimelineManager.Instance.instanceIds)
            {
                TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
                HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
                hitboxComponent.CollisionBuffer.Clear();
            }
        }
    }
}