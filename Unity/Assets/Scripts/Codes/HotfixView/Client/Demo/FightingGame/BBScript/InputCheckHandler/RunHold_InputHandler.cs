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

        public override async ETTask<InputBuffer> Handle(InputWait self, ETCancellationToken token)
        {
            WaitInput wait = await self.Wait(OP: BBOperaType.LEFT | BBOperaType.RIGHT, FuzzyInputType.OR);
            if (wait.Error is not WaitTypeError.Success)
            {
                return InputBuffer.None;
            }

            return self.CreateBuffer( 4);
        }
    }
}