using UnityEngine;

namespace ET
{
    public class SceneEnemy : MonoBehaviour
    {
        //Unit.InstanceId
        [HideInInspector]
        public long instanceId;
    }

    public struct EditSceneEnemyCallback
    {
        public long instanceId;
    }
}