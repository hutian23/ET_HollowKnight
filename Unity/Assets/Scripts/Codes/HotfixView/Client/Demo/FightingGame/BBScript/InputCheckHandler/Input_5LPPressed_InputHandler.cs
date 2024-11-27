namespace ET.Client
{
    public class Input_5LPPressed_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "5LPPressed";
        }

        public override async ETTask<InputStatus> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);

            WaitInput wait = await inputWait.Wait(OP: BBOperaType.LIGHTPUNCH, FuzzyInputType.OR, () =>
            {
                //避免闭包
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.LIGHTPUNCH);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputStatus.Failed;
            
            return InputStatus.Success;
        }
    }
}