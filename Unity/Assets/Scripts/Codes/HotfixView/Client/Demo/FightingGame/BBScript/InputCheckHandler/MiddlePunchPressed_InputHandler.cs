namespace ET.Client
{
    public class MiddlePunchPressed_InputHandler : BBInputHandler
    {
        public override string GetInputType()
        {
            return "MiddlePunchPressed";
        }

        public override async ETTask<InputStatus> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);

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