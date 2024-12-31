using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEditor;
using UnityEngine;

namespace Timeline
{
    [CreateAssetMenu(menuName = "ScriptableObject/BBTimeline/PlayableGraph", fileName = "BBPlayableGraph")]
    public class BBPlayableGraph: SerializedScriptableObject
    {
        public TextAsset BBScript;
        
        [OdinSerialize]
        public Dictionary<string, BBTimeline> timelineDict = new();
        
#if UNITY_EDITOR
        private SerializedObject SerializedController;

        public void SerializedUpdate()
        {
            SerializedController = new SerializedObject(this);
            SerializedController.Update();
        }
#endif

        public BBTimeline GetTimeline(string timelineName)
        {
            if (!timelineDict.TryGetValue(timelineName, out BBTimeline timeline))
            {
                Debug.LogError($"does not exist timeline: {timelineName}");
            }
            return timeline;
        }
    }
}