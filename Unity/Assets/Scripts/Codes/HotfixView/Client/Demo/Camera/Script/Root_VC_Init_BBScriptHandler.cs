using UnityEngine;

namespace ET.Client
{
    public class Root_VC_Init_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "VC_Init";
        }

        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            Unit unit = parser.GetParent<Unit>();
            GameObject go = unit.GetComponent<GameObjectComponent>().GameObject;
            
            //1. 如果存在_Confiner子物体
            GameObject _confiner = go.transform.Find("_Confiner").gameObject;
            if (_confiner != null)
            {
                b2BoxCollider2D _box = _confiner.GetComponent<b2BoxCollider2D>();
                parser.RegistParam("VC_Confiner_Center", _box.info.center);
                parser.RegistParam("VC_Confiner_Size", _box.info.size);
            }
            
            //2.
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}