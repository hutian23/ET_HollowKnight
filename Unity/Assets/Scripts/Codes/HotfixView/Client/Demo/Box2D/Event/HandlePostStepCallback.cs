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
            b2GameManager.Instance.B2World.PostStepCallback?.Invoke();
            b2GameManager.Instance.B2World.PostStepCallback = null;
        }
    }
}