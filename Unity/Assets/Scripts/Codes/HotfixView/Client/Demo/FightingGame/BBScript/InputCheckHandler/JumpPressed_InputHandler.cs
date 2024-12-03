namespace ET.Client
{
    public class JumpPressed_InputHandler: BBInputHandler
    {
        public override string GetHandlerType()
        {
            return "JumpPressed";
        }

        public override string GetBufferType()
        {
            return "JumpPressed";
        }

        public override async ETTask<InputBuffer> Handle(InputWait inputWait, ETCancellationToken token)
        {
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.A, FuzzyInputType.OR, () =>
            {
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.A);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputBuffer.None;

            return inputWait.CreateBuffer(10);
        }
    }
}