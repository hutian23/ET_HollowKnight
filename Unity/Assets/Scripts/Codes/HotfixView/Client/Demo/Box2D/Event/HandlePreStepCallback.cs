namespace ET.Client
{
    [Invoke]
    public class HandlePreStepCallback : AInvokeHandler<PreStepCallback>
    {
        public override void Handle(PreStepCallback args)
        {
            b2WorldManager.Instance.GetPreStepTimer().Step();
            EventSystem.Instance.PreStepUpdate();
        }
    }
}