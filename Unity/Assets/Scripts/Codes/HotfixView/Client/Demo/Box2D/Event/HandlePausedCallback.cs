using Testbed.Abstractions;

namespace ET.Client
{
    [Invoke]
    public class HandlePausedModeCallback: AInvokeHandler<PausedCallback>
    {
        public override void Handle(PausedCallback args)
        {
            Global.Settings.Pause = args.Pause;
            //抛出事件?
            // loader层只能调用回调，不能抛出事件
            EventSystem.Instance.PublishAsync(ClientSceneManagerComponent.Instance.Get(1), new PausedCallback() { Pause = args.Pause }).Coroutine();
        }
    }
}