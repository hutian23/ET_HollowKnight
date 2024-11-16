using Box2DSharp.Dynamics;

namespace ET.Client
{
    [Event(SceneType.Client)]
    [FriendOf(typeof(b2Body))]
    [FriendOf(typeof(InputWait))]
    [FriendOf(typeof(HitboxComponent))]
    public class BeforeBehaviorReload_UpdateFlip : AEvent<BeforeBehaviorReload>
    {
        protected override async ETTask Run(Scene scene, BeforeBehaviorReload args)
        {
            Unit unit = Root.Instance.Get(args.instanceId) as Unit;
            b2Body b2Body = b2GameManager.Instance.GetBody(unit.InstanceId);
            InputWait inputWait = unit.GetComponent<TimelineComponent>().GetComponent<InputWait>();
            
            //1. Dispose Hitbox
            for (int i = 0; i < b2Body.hitBoxFixtures.Count; i++)
            {
                Fixture fixture = b2Body.hitBoxFixtures[i];
                b2Body.body.DestroyFixture(fixture);
            }  
            b2Body.hitBoxFixtures.Clear();
            
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

            // update flip
            if (curFlag != preFlag)
            {
                EventSystem.Instance.Invoke(new UpdateFlipCallback() { instanceId = b2Body.unitId,curFlip = curFlag});
            }
            await ETTask.CompletedTask;
        }
    }
}