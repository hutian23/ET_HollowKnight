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

        public override async ETTask<InputStatus> Handle(InputWait inputWait, ETCancellationToken token)
        {
            WaitInput wait = await inputWait.Wait(BBOperaType.HEAVYPUNCH, FuzzyInputType.OR, () =>
            {
                bool wasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.HEAVYPUNCH);
                return wasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputStatus.Failed;
            
            return InputStatus.Success;
        }
    }
}