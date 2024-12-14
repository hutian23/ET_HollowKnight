namespace ET.Client
{
    [FriendOf(typeof(b2Body))]
    public class Hit_GotoState_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "HitStun";
        }

        //Hit_GotoState: 'KnockBack';
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}