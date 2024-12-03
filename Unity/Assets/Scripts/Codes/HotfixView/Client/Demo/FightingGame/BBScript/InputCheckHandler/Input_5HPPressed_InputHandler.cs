namespace ET.Client
{
    public class Input_5HPPressed_InputHandler : BBInputHandler
    {
        public override string GetHandlerType()
        {
            return "5HPPressed";
        }

        public override string GetBufferType()
        {
            return "5HPPressed";
        }

        public override async ETTask<InputBuffer> Handle(InputWait inputWait, ETCancellationToken token)
        {
            WaitInput wait = await inputWait.Wait(BBOperaType.RB, FuzzyInputType.OR, () =>
            {
                bool wasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.RB);
                return wasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputBuffer.None;
            
            return InputBuffer.None;
        }
    }
}