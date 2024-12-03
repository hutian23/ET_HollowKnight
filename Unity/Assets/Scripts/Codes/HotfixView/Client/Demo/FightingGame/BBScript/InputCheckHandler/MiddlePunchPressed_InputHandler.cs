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

        public override async ETTask<InputBuffer> Handle(InputWait inputWait, ETCancellationToken token)
        {
            //1. Wait 
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.Y, FuzzyInputType.OR, () =>
            {
                //避免闭包
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.Y);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputBuffer.None;

            return inputWait.DefaultBuffer();
        }
    }
}