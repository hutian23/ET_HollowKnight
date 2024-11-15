namespace ET.Client
{
    public class Input_2MP_InputHandler : BBInputHandler
    {
        public override string GetInputType()
        {
            return "2MP";
        }
        
        public override async ETTask<Status> Handle(Unit unit, ETCancellationToken token)
        {
            InputWait inputWait = BBInputHelper.GetInputWait(unit);
            
            //1. Wait Down
            // WaitInput wait = await inputWait.Wait(OP: BBOperaType.DOWN | BBOperaType.DOWNLEFT | BBOperaType.DOWNRIGHT, FuzzyInputType.OR, () =>
            // {
            //     
            // });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}