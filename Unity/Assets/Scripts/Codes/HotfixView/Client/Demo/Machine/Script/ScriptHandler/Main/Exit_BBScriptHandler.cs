namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(BehaviorMachine))]
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
            Unit unit = parser.GetParent<Unit>();
            BehaviorMachine machine = unit.GetComponent<BehaviorMachine>();

            // 下一帧执行
            await TimerComponent.Instance.WaitFrameAsync(token);
            if (token.IsCancel()) return Status.Failed;
            
            int targetOrder = 0;
            foreach (long infoId in machine.DescendInfoList)
            {
                BehaviorInfo info = machine.GetChild<BehaviorInfo>(infoId);
                if (info.moveType is MoveType.HitStun || info.moveType is MoveType.Etc)
                {
                    continue;
                }
                if (info.Trigger())
                {
                    targetOrder = info.behaviorOrder;
                    break;
                }
            }

            machine.Reload(targetOrder);
            
            await ETTask.CompletedTask;
            return Status.Return;
        }
    }
}