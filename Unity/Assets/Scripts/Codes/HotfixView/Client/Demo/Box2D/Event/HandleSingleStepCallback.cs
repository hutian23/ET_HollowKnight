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

            //1. 更新timer
            BBTimerManager.Instance.Step();
            
            //2. 物理层更新一帧
            b2WorldManager.Instance.Step();
        }
    }
}