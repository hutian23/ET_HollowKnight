namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(InputWait))]
    [FriendOf(typeof(b2Unit))]
    public class BeforeBehaviorReload_Player_ReloadB2body : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            b2Body b2Body = b2WorldManager.Instance.GetBody(unit.InstanceId);
            b2Unit b2Unit = unit.GetComponent<TimelineComponent>().GetComponent<b2Unit>();
            InputWait inputWait = unit.GetComponent<TimelineComponent>().GetComponent<InputWait>();
            
            //1. 销毁旧夹具
            b2Body.ClearHitbox();
            //2. 更新hitbox
            b2Unit.Init();
            //3. 更新朝向
            if (inputWait.IsPressing(BBOperaType.LEFT) ||
                inputWait.IsPressing(BBOperaType.UPLEFT) ||
                inputWait.IsPressing(BBOperaType.DOWNLEFT))
            {
                b2Body.Flip = FlipState.Left;
            }
            else if (inputWait.IsPressing(BBOperaType.RIGHT) ||
                     inputWait.IsPressing(BBOperaType.UPRIGHT) ||
                     inputWait.IsPressing(BBOperaType.DOWNRIGHT))
            {
                b2Body.Flip = FlipState.Right;
            }
            b2Body.UpdateFlag = true;
            
            await ETTask.CompletedTask;
        }
    }
}