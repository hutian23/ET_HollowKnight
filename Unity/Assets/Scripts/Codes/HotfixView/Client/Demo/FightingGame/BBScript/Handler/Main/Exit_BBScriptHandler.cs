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
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BehaviorBuffer buffer = timelineComponent.GetComponent<BehaviorBuffer>();

            int targetOrder = 0;
            foreach (var infoId in buffer.DescendInfoList)
            {
                BehaviorInfo info = buffer.GetChild<BehaviorInfo>(infoId);
                if (info.moveType is MoveType.HitStun || info.moveType is MoveType.Etc)
                {
                    continue;
                }
                if (info.BehaviorCheck())
                {
                    targetOrder = info.behaviorOrder;
                    break;
                }
            }
            
            timelineComponent.Reload(targetOrder);
            
            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}