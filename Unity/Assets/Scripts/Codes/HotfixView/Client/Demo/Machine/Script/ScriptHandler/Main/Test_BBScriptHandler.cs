using MongoDB.Bson;
using UnityEngine;
using Camera = UnityEngine.Camera;

namespace ET.Client
{
    [FriendOf(typeof(B2Unit))]
    [FriendOf(typeof(InputWait))]
    public class Test_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "Test";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            // Vector3 half = new(HalfWidth, HalfHeight);
            //
            // Unit unit = Root.Instance.Get(parser.GetParam<long>("VC_Follow_Id")) as Unit;
            // GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            //
            // //1. 先不考虑死区的问题
            // Rect curRect = new(go.transform.position - half , new Vector2(HalfWidth, HalfHeight) * 2);
            // Rect confiner = parser.GetParam<Rect>("VC_Confiner_Rect");
            // Log.Warning(confiner.ToJson() + "   "+ curRect.ToJson());
            //
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}