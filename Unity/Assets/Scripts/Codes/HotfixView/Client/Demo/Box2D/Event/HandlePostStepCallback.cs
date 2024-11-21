namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(TimelineManager))]
    [FriendOf(typeof(HitboxComponent))]
    public class HandlePostStepCallback : AInvokeHandler<PostStepCallback>
    {
        public override void Handle(PostStepCallback args)
        {
            foreach (long instanceId in TimelineManager.Instance.instanceIds)
            {
                TimelineComponent timelineComponent = Root.Instance.Get(instanceId) as TimelineComponent;
                HitboxComponent hitboxComponent = timelineComponent.GetComponent<HitboxComponent>();
                BBParser bbParser = timelineComponent.GetComponent<BBParser>();
         
                //1. 碰撞回调
                foreach (string callbackName in hitboxComponent.callbackSet)
                {
                    CollisionCallback callback = DialogueDispatcherComponent.Instance.GetCollisionCallback(callbackName);
                    callback.Handle(bbParser);
                }
                
                //2. 清空碰撞信息缓冲区
                hitboxComponent.infoQueue.Clear();
            }
        }
    }
}