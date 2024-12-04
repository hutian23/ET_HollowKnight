namespace ET.Client
{
    [FriendOf(typeof(InputWait))]
    public class Modern_ShouRyuKen_InputHandler : BBInputHandler
    {
        public override string GetHandlerType()
        {
            return "Modern_ShouRyuKen";
        }

        public override string GetBufferType()
        {
            return "ShouRyuKen";
        }

        public override async ETTask<InputBuffer> Handle(InputWait self, ETCancellationToken token)
        {
            WaitInput wait = await self.Wait(OP: BBOperaType.X, waitType: FuzzyInputType.OR);
            if (wait.Error != WaitTypeError.Success)
            {
                return InputBuffer.None;
            }
            return self.CreateBuffer(15);
        }
    }
}