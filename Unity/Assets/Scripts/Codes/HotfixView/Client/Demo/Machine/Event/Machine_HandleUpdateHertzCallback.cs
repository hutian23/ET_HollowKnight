namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBTimerComponent))]
    [FriendOf(typeof(BBParser))]
    public class Machine_HandleUpdateHertzCallback : AInvokeHandler<BehaviorUpdateHertzCallback>
    {
        public override void Handle(BehaviorUpdateHertzCallback args)
        {
            BehaviorMachine machine = Root.Instance.Get(args.instanceId) as BehaviorMachine;
            if (machine == null || machine.InstanceId == 0)
            {
                Log.Error($"cannot found unit:{args.instanceId}!");
                return;
            }

            Unit unit = machine.GetParent<Unit>();
            BBTimerComponent bbTimer = unit.GetComponent<BBTimerComponent>();
            B2Unit b2Unit = unit.GetComponent<B2Unit>();
            BBParser parser = unit.GetComponent<BBParser>();

            //1. 更新战斗定时器timeScale
            bbTimer.SetHertz(args.hertz);
            bbTimer.Accumulator = 0;

            //2. 速度随timeScale缩放
            b2Unit.SetHertz(args.hertz);

            //参考HandleBeforeBehaviorReloadCallback
            if (!parser.ContainFunction("Root", "UpdateHertz")) return;
            parser.RegistParam("Hertz", args.hertz);
            parser.Invoke(parser.GetFunctionPointer("Root", "UpdateHertz"), parser.CancellationToken).Coroutine();
            parser.TryRemoveParam("Hertz");
        }
    }
}