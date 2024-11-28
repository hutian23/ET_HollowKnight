namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(InputWait))]
    [FriendOf(typeof(HitboxComponent))]
    public class BeforeBehaviorReload_PlayerReloadB2body : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            b2Body b2Body = b2GameManager.Instance.GetBody(unit.InstanceId);
            InputWait inputWait = unit.GetComponent<TimelineComponent>().GetComponent<InputWait>();
            HitboxComponent hitboxComponent = unit.GetComponent<TimelineComponent>().GetComponent<HitboxComponent>();
            
            //1. 销毁旧夹具
            b2Body.ClearHitbox();
            
            //2. 转向
            FlipState preFlag = b2Body.Flip;
            FlipState curFlag = preFlag;
            if (inputWait.ContainKey(BBOperaType.LEFT) || 
                inputWait.ContainKey(BBOperaType.UPLEFT) || 
                inputWait.ContainKey(BBOperaType.DOWNLEFT))
            {
                curFlag = FlipState.Left;
            }
            else if (inputWait.ContainKey(BBOperaType.RIGHT) ||
                     inputWait.ContainKey(BBOperaType.DOWNRIGHT) ||
                     inputWait.ContainKey(BBOperaType.UPRIGHT))
            {
                curFlag = FlipState.Right;
            }
            if (curFlag != preFlag)
            {
                EventSystem.Instance.Invoke(new UpdateFlipCallback() { instanceId = b2Body.unitId,curFlip = curFlag});
            }
            
            //3. 更新hitbox
            hitboxComponent.Init();
            
            await ETTask.CompletedTask;
        }
    }
}