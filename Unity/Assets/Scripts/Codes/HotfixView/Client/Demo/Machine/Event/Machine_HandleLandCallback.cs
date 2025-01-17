namespace ET.Client
{
    [Invoke]
    [FriendOf(typeof(BBParser))]
    public class Machine_HandleLandCallback : AInvokeHandler<LandCallback>
    {
        public override void Handle(LandCallback args)
        {
            BehaviorMachine machine = Root.Instance.Get(args.instanceId) as BehaviorMachine;
            if (machine == null || machine.InstanceId == 0)
            {
                Log.Error($"cannot found behavior machine: {args.instanceId}");
                return;
            }

            Unit unit = machine.GetParent<Unit>();
            BBParser parser = unit.GetComponent<BBParser>();

            if (!parser.ContainFunction("Root", "LandCallback")) return;
            parser.Invoke(parser.GetFunctionPointer("Root", "LandCallback"), parser.CancellationToken).Coroutine();

        }
    }
}