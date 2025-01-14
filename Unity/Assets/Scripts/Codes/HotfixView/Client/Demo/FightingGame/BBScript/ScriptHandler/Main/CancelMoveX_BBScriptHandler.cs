﻿namespace ET.Client
{
    public class CancelMoveX_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CancelMoveX";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            BBParser bbParser = timelineComponent.GetComponent<BBParser>();
            BBTimerComponent bbTimer = timelineComponent.GetComponent<BBTimerComponent>();

            long timer = bbParser.GetParam<long>("MoveXTimer");
            bbTimer.Remove(ref timer);
            bbParser.RemoveParam("MoveXTimer");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}