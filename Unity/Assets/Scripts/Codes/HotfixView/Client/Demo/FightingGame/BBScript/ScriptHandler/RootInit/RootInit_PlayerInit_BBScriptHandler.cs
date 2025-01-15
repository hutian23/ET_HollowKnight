namespace ET.Client
{
    public class RootInit_PlayerInit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "PlayerInit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit player = parser.GetParent<Unit>();

            player.AddComponent<TimelineComponent>();
            player.AddComponent<BBTimerComponent>().IsFrameUpdateTimer();
            player.AddComponent<BBNumeric>();
            player.AddComponent<BehaviorBuffer>();
            player.AddComponent<B2Unit, long>(player.InstanceId);
            player.AddComponent<ObjectWait>();
            player.AddComponent<InputWait>();
            
            token.Add(() =>
            {
                player.RemoveComponent<TimelineComponent>();
                player.RemoveComponent<BBTimerComponent>();
                player.RemoveComponent<BBNumeric>();
                player.RemoveComponent<BehaviorBuffer>();
                player.RemoveComponent<B2Unit>();
                player.RemoveComponent<ObjectWait>();
                player.RemoveComponent<InputWait>();
            });
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}