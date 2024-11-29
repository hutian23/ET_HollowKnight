namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorBuffer))]
    [FriendOf(typeof(BehaviorInfo))]
    public class Exit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Exit";
        }

        //Exit;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(parser.GetEntityId()) as TimelineComponent;
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            buffer.SetCurrentOrder(-1);
            foreach (long id in buffer.DescendInfoList)
            {
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(id);
                if (info.moveType is MoveType.Etc || info.moveType is MoveType.HitStun)
                {
                    continue;
                }

                if (info.BehaviorCheck())
                {
                    timelineComponent.Reload(info.behaviorOrder);
                    break;
                }
            }

            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}