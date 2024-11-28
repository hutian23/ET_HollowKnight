namespace ET.Client
{
    public class RunHold_InputHandler: BBInputHandler
    {
        public override string GetInputType()
        {
            return "RunHold";
        }

        //关于需要持续按住按键的操作可以参考如下
        public override async ETTask<InputStatus> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);
            WaitInput wait = await inputWait.Wait(OP: BBOperaType.LEFT | BBOperaType.RIGHT, FuzzyInputType.OR);
            if (wait.Error is not WaitTypeError.Success)
            {
                return InputStatus.Failed;
            }
            
            return new InputStatus(){buffFrame = 5,ret = Status.Success};
        }
    }
}