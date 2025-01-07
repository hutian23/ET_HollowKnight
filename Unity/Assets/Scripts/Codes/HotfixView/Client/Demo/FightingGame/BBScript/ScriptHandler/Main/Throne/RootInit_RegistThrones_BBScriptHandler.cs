using Timeline;
using UnityEngine;

namespace ET.Client
{
    public class RootInit_RegistThrones_BBScriptHandler : BBScriptHandler
    {
        public override string GetOPType()
        {
            return "RegistThrones";
        }

        //ThroneState: 1, Boss_WallThrow;
        public override async ETTask<Status> Handle(BBParser parser, BBScriptData data, ETCancellationToken token)
        {
            TimelineComponent timelineComponent = parser.GetParent<TimelineComponent>();
            GameObject go = timelineComponent.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject;
            ReferenceCollector refer = go.GetComponent<ReferenceCollector>();
            
            GameObject throneGo1 = refer.Get<GameObject>("Throne_1");
            TimelinePlayer timelinePlayer1 = throneGo1.GetComponent<TimelinePlayer>();
            timelineComponent.RegistParam("Throne_1", timelinePlayer1.instanceId);
            
            GameObject throneGo2 = refer.Get<GameObject>("Throne_2");
            TimelinePlayer timelinePlayer2 = throneGo2.GetComponent<TimelinePlayer>();
            timelineComponent.RegistParam("Throne_2", timelinePlayer2.instanceId);
            
            GameObject throneGo3 = refer.Get<GameObject>("Throne_3");
            TimelinePlayer timelinePlayer3 = throneGo3.GetComponent<TimelinePlayer>();
            timelineComponent.RegistParam("Throne_3", timelinePlayer3.instanceId);
            
            await ETTask.CompletedTask;
            return Status.Success;
        }
    }
}