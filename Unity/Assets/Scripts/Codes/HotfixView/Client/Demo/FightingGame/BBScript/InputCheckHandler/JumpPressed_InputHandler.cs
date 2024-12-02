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

        public override async ETTask<InputStatus> Handle(InputWait inputWait, ETCancellationToken token)
        {
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.LIGHTKICK, FuzzyInputType.OR, () =>
            {
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.LIGHTKICK);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputStatus.Failed;
            
            return new InputStatus(){buffFrame = 8, ret = Status.Success};
        }
    }
}