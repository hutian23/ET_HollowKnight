namespace ET.Client
{
    public class SquitHold_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "SquitHold";
        }

        public override async ETTask<InputStatus> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);
            
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.DOWN | BBOperaType.DOWNLEFT | BBOperaType.DOWNRIGHT, FuzzyInputType.OR);
            if (wait.Error is not WaitTypeError.Success)
            {
                return InputStatus.Failed;
            }
            
            return InputStatus.Success;
        }
    }
}