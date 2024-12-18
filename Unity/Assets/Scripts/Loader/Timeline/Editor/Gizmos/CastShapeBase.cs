using UnityEngine;

namespace Timeline.Editor
{
    public abstract class CastShapeBase : MonoBehaviour, ITimelineGenerate
    {
#if UNITY_EDITOR
        protected readonly Color m_gizmosColor = Color.cyan;

        protected virtual void OnDrawGizmos()
        {
            
        }
#endif
    }
}