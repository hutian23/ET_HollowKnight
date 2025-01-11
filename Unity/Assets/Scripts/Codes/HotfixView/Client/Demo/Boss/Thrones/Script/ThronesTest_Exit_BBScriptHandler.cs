namespace ET.Client
{
    public class ThronesTest_Exit_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "ThronesTest_Exit";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            BBTimerComponent bbTimer = BBTimerManager.Instance.SceneTimer();
            long timer = parser.GetParam<long>("ThronesTestTimer");
            bbTimer.Remove(ref timer);
            
            //clear dead flag
            long instanceId_1 = parser.GetParam<long>($"Throne_1");
            TimelineComponent timelineComponent_1 = Root.Instance.Get(instanceId_1) as TimelineComponent;
            timelineComponent_1.TryRemoveParam("DeadFlag");
            
            long instanceId_2 = parser.GetParam<long>($"Throne_2");
            TimelineComponent timelineComponent_2 = Root.Instance.Get(instanceId_2) as TimelineComponent;
            timelineComponent_2.TryRemoveParam("DeadFlag");
            
            long instanceId_3 = parser.GetParam<long>("Throne_3");
            TimelineComponent timelineComponent_3 = Root.Instance.Get(instanceId_3) as TimelineComponent;
            timelineComponent_3.TryRemoveParam("DeadFlag");
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}