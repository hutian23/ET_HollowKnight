using UnityEngine;

namespace ET.Client
{
    [FriendOf(typeof(BBParser))]
    [FriendOf(typeof(EnemyManager))]
    public class CreateBall_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "CreateBall";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            //1. 跳过代码块
            int index = parser.Coroutine_Pointers[data.CoroutineID];
            int endIndex = index, startIndex = index;
            while (++index < parser.opDict.Count)
            {
                string opLine = parser.opDict[index];
                if (opLine.Equals("EndCreateBall:"))
                {
                    endIndex = index;
                    break;
                }
            }
            parser.Coroutine_Pointers[data.CoroutineID] = endIndex;

            //2. 执行代码块
            Status ret = await parser.RegistSubCoroutine(startIndex, endIndex, token);
            if (token.IsCancel() || ret != Status.Success) return Status.Failed;

            //3. 
            string ballType = parser.GetParam<string>("BallType");
            // Vector2 ballPos = parser.GetParam<Vector2>("BallPos");
            // Vector2 ballStartV = parser.GetParam<Vector2>("BallStartV");

            //3. 创建子弹unit
            UnitComponent unitComponent = parser.ClientScene().CurrentScene().GetComponent<UnitComponent>();
            //3-1 对象池获得GameObject
            Unit ball = unitComponent.AddChild<Unit, int>(1001);
            GameObject ballGo = GameObjectPoolHelper.GetObjectFromPool(ballType);
            ballGo.transform.SetParent(GlobalComponent.Instance.Unit);
            ball.AddComponent<GameObjectComponent>().GameObject = ballGo;

            TimelineComponent timelineComponent = ball.AddComponent<TimelineComponent>();
            timelineComponent.AddComponent<BBTimerComponent>().IsFrameUpdateTimer();
            timelineComponent.AddComponent<b2Unit, long>(ball.InstanceId);
            timelineComponent.AddComponent<ObjectWait>();
            timelineComponent.AddComponent<BBParser>();
            timelineComponent.AddComponent<BehaviorBuffer>();

            // 热重载时销毁ball
            EnemyManager.Instance.ReloadQueue.Enqueue(ball.InstanceId);

            return Status.Success;
        }
    }
}