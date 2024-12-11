using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    public class HandleSingleStepCallback : AInvokeHandler<SingleStepCallback>
    {
        public override void Handle(SingleStepCallback args)
        {
            if (!Global.Settings.Pause)
            {
                return;
            }
            
            BBTimerManager.Instance.Step();
        }
    }
}