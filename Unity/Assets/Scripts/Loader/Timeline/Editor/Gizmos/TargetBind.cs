using UnityEngine;

namespace Timeline.Editor
{
    public class TargetBind: MonoBehaviour, ITimelineGenerate
    {
        [Sirenix.OdinInspector.ReadOnly]
        public string TrackName;
    }
}