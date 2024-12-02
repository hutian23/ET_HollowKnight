namespace ET.Client
{
    public class SquatHold_InputHandler: BBInputHandler
    {
        public override string GetHandlerType()
        {
            return "SquatHold";
        }

        public override string GetBufferType()
        {
            return "SquatHold";
        }

        public override async ETTask<InputStatus> Handle(InputWait inputWait, ETCancellationToken token)
        {
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.DOWN | BBOperaType.DOWNLEFT | BBOperaType.DOWNRIGHT, FuzzyInputType.OR);
            if (wait.Error is not WaitTypeError.Success)
            {
                return InputStatus.Failed;
            }
            
            return InputStatus.Success;
        }
    }
}