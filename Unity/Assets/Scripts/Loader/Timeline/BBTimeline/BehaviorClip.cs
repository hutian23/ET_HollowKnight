﻿using System;
using UnityEngine;

namespace Timeline
{
    [Serializable]
    public class BehaviorClip
    {
        public BBTimeline Timeline;

        public bool IsDefault;
        public string Layer;
        public string Title;

        [TextArea(10, 30)]
        public string Script;

#if UNITY_EDITOR
        public string viewDataKey;
        public Vector3 ClipPos;
#endif
    }

    [Serializable]
    public class BehaviorLayer
    {
        public string layerName;
    }

#if UNITY_EDITOR
    [Serializable]
    public class BehaviorLinkData
    {
        public string viewDataKey;
        public string inputGuid;
        public string outputGuid;
    }
#endif
}