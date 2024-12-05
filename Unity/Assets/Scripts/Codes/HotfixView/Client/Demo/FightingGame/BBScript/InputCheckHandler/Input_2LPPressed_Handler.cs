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

        public override long Handle(InputWait self)
        {
            bool direction = self.IsKeyCached(BBOperaType.DOWN) || self.IsKeyCached(BBOperaType.DOWNLEFT) || self.IsKeyCached(BBOperaType.DOWNRIGHT);
            return direction && self.IsPressing(BBOperaType.X)? self.GetBuffFrame(10): -1;
        }
    }
}