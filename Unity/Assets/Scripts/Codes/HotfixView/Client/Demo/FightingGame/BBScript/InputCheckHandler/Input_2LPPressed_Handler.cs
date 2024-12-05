namespace ET.Client
{
    [FriendOf(typeof(InputWait))]
    public class Input_2LPPressed_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "2LPPressed";
        }

        public override string GetBufferType()
        {
            return "2LPPressed";
        }

        public override int Handle(InputWait self)
        {
            return -1;
        }
    }
}