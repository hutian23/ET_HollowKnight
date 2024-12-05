namespace ET.Client
{
    public class Input_DashPressed_Handler: InputHandler
    {
        public override string GetHandlerType()
        {
            return "DashPressed";
        }

        public override string GetBufferType()
        {
            return "DashPressed";
        }

        public override int Handle(InputWait self)
        {
            return self.IsPressed(BBOperaType.RT)? 10 : -1;
        }
    }
}