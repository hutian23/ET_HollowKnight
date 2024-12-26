using UnityEngine;

namespace ET
{
    public class SceneEnemy : MonoBehaviour
    {
        //Unit.InstanceId
        public long instanceId;
    }

    public struct EditSceneEnemyCallback
    {
        public long instanceId;
    }
}