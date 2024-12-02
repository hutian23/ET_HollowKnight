namespace ET.Client
{
    public class RunHold_InputHandler: BBInputHandler
    {
        public override string GetHandlerType()
        {
            return "RunHold";
        }

        public override string GetBufferType()
        {
            return "RunHold";
        }

        public override async ETTask<InputStatus> Handle(InputWait inputWait, ETCancellationToken token)
        {
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.LEFT | BBOperaType.RIGHT, FuzzyInputType.OR);
            if (wait.Error is not WaitTypeError.Success)
            {
                return InputStatus.Failed;
            }
            
            return new InputStatus(){buffFrame = 5,ret = Status.Success};
        }
    }
}