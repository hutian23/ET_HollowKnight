﻿namespace ET.Client
{
    public class Input_RunHold_Handler : InputHandler
    {
        public override string GetHandlerType()
        {
            return "RunHold";
        }

        public override string GetBufferType()
        {
            return "RunHold";
        }

        //同步检测时，只需要判断当前帧是否按下即可
        public override long Handle(InputWait self)
        {
            return self.IsPressing(BBOperaType.LEFT) || self.IsPressing(BBOperaType.RIGHT)? self.GetBuffFrame(4) : -1;
        }
    }
}