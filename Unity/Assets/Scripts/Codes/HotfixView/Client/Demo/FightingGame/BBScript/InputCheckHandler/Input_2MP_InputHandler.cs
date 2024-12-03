namespace ET.Client
{
    public class Input_2MPPressed_InputHandler : BBInputHandler
    {
        public override string GetHandlerType()
        {
            return "2MPPressed";
        }

        public override string GetBufferType()
        {
            return "2MPPressed";
        }

        public override async ETTask<InputBuffer> Handle(InputWait inputWait, ETCancellationToken token)
        {
            //1. Wait attack input
            WaitInput wait = await inputWait.Wait(BBOperaType.Y, FuzzyInputType.OR, () =>
            {
                //避免闭包
                bool WasPressedThisFrame = inputWait.WasPressedThisFrame(BBOperaType.Y);
                return WasPressedThisFrame;
            });
            if (wait.Error != WaitTypeError.Success) return InputBuffer.None;
            
            //2. 按下攻击键的同一帧，判断是否包含以下输入
            long op = wait.OP;
            if ((op & BBOperaType.DOWN) != 0 ||
                (op & BBOperaType.DOWNLEFT) != 0||
                (op & BBOperaType.DOWNRIGHT) != 0)
            {
                return inputWait.DefaultBuffer();
            }
            return InputBuffer.None;
        }
    }
}