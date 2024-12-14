namespace ET.Client
{
    [Invoke]
    public class HandlePostStepCallback : AInvokeHandler<PostStepCallback>
    {
        public override void Handle(PostStepCallback args)
        {
            b2WorldManager.Instance.GetPostStepTimer().Step();
            EventSystem.Instance.PostStepUpdate();
        }
    }
}