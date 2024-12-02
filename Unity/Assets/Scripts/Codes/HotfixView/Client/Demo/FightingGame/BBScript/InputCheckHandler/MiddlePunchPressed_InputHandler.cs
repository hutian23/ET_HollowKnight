namespace ET.Client
{
    public class MiddlePunchPressed_InputHandler : BBInputHandler
    {
        public override string GetHandlerType()
        {
            return "MiddlePunchPressed";
        }

        public override string GetBufferType()
        {
            return "MiddlePunchPressed";
        }

        public override async ETTask<InputStatus> Handle(InputWait inputWait, ETCancellationToken token)
        {
            //1. Wait 
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.MIDDLEPUNCH, FuzzyInputType.OR, () =>
            {
                //避免闭包
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.MIDDLEPUNCH);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputStatus.Failed;

            return InputStatus.Success;
        }
    }
}