namespace ET.Client
{
    [Invoke(BBTimerInvokeType.CameraMoveToTimer)]
    public class CameraMoveToTimer : BBTimer<BBParser>
    {
        protected override void Run(BBParser self)
        {
            Log.Warning("Hello world!!!");
        }
    }
}