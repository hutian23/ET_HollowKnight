namespace ET.Client
{
    public class DashPressed_InputHandler : BBInputHandler
    {
        public override string GetHandlerType()
        {
            return "DashPressed";
        }

        public override string GetBufferType()
        {
            return "DashPressed";
        }

        public override async ETTask<InputStatus> Handle(InputWait inputWait, ETCancellationToken token)
        {
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.HEAVYKICK, FuzzyInputType.AND, () =>
            {
                //避免闭包
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.HEAVYKICK);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputStatus.Failed;
            
            return new InputStatus(){buffFrame = 7,ret = Status.Success};
        }
        
    }
}