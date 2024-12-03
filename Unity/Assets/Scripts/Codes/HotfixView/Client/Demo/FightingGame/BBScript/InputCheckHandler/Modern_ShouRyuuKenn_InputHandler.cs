namespace ET.Client
{
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
            WaitInput wait = await self.Wait(OP: BBOperaType.LB | BBOperaType.Y , waitType: FuzzyInputType.AND, () =>
            {
                bool WasPressedThisFrame = self.WasPressedThisFrame(BBOperaType.Y);
                return WasPressedThisFrame;
            });
            return wait.Error != WaitTypeError.Success? InputBuffer.None : self.CreateBuffer(15);
        }
    }
}