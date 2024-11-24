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
            //1. 更新PostStep声明周期函数
            b2GameManager.Instance.GetPostStepTimer().Step();
            
            //2. PreStep或Step生命周期中不能操作夹具，可以添加到PostStep回调
            b2GameManager.Instance.B2World.PostStepCallback?.Invoke();
            b2GameManager.Instance.B2World.PostStepCallback = null;

            //清空碰撞缓冲区
            //TODO hitbox 的postStep生命周期函数
            foreach (long instanceId in TimelineManager.Instance.instanceIds)
            {
                TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
                HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
                hitboxComponent.CollisionBuffer.Clear();
            }
        }
    }
}