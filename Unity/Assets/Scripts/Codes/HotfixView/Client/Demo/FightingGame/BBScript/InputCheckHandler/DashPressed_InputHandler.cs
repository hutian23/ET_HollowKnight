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

        public override async ETTask<InputBuffer> Handle(InputWait self, ETCancellationToken token)
        {
            WaitInput wait = await self.Wait(OP: BBOperaType.RT, FuzzyInputType.AND, () =>
            {
                //避免闭包
                bool WasPressedThisFrame = self.WasPressedThisFrame(BBOperaType.RT);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputBuffer.None;

            return self.CreateBuffer(7);
        }
        
    }
}