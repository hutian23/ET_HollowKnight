namespace ET.Client
{
    [Event(SceneType.Client)]
    public class OnGroundChanged_ResetParam : AEvent<OnGroundChanged>
    {
        protected override async ETTask Run(Scene scene, OnGroundChanged args)
        {
            TimelineComponent timelineComponent = Root.Instance.Get(args.instanceId) as TimelineComponent;
            
            //恢复 AirDash
            int maxDash = (int)timelineComponent.GetParam<long>("MaxDash");
            timelineComponent.UpdateParam("DashCount",maxDash);
            
            //恢复 jump
            int jump = (int)timelineComponent.GetParam<long>("MaxJump");
            timelineComponent.UpdateParam("JumpCount", jump);
            
            await ETTask.CompletedTask;
        }
    }
}