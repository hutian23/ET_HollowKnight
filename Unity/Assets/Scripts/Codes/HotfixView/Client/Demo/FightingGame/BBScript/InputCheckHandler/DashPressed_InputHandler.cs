namespace ET.Client
{
    public class DashPressed_InputHandler : BBInputHandler
    {
        public override string GetInputType()
        {
            return "DashPressed";
        }

        public override async ETTask<InputStatus> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);

            WaitInput wait = await inputWait.Wait(OP: BBOperaType.HEAVYKICK, FuzzyInputType.AND, () =>
            {
                //避免闭包
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.HEAVYKICK);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputStatus.Failed;
            
            return new InputStatus(){buffFrame = 10,ret = Status.Success};
        }
    }
}