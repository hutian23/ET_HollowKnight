namespace ET.Client
{
    public class Input_5LPPressed_InputHandler: BBInputHandler
    {
        public override string GetHandlerType()
        {
            return "5LPPressed";
        }

        public override string GetBufferType()
        {
            return "5LPPressed";
        }

        public override async ETTask<InputBuffer> Handle(InputWait inputWait, ETCancellationToken token)
        {
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.X, FuzzyInputType.OR, () =>
            {
                //避免闭包
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.X);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputBuffer.None;

            return inputWait.CreateBuffer(15);
        }
    }
}