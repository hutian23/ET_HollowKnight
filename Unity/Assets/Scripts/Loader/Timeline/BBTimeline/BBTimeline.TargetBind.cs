using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline.Editor;
using UnityEngine;

namespace Timeline
{
    [Serializable]
    [BBTrack("TargetBind")]
#if UNITY_EDITOR
    [Color(100, 100, 100)]
    [IconGuid("51d6e4824d3138c4880ca6308fa0e473")]
#endif
    public class BBTargetBindTrack: BBTrack
    {
        [NonSerialized,OdinSerialize]
        public List<TargetBindInfo> TargetBindInfos = new();
        
        public override Type RuntimeTrackType => typeof (RuntimeTargetBindTrack);

        public TargetBindInfo GetInfo(int targetFrame)
        {
            return TargetBindInfos.FirstOrDefault(info => info.frame == targetFrame);
        }
        
#if UNITY_EDITOR
        public override Type TrackViewType => typeof (TargetBindTrackView);

        public override int GetMaxFrame()
        {
            return TargetBindInfos.Count > 0 ? TargetBindInfos.Max(info => info.frame) : 0;
        }
#endif
    }

    [Serializable]
    public class TargetBindInfo: BBKeyframeBase
    {
        public Vector2 TargetPosition;
    }
    
    #region Runtime

    public class RuntimeTargetBindTrack: RuntimeTrack
    {
        public RuntimeTargetBindTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
            
        }

        public override void UnBind()
        {
  
        }

        public override void SetTime(int targetFrame)
        {
        }
    }

    #endregion

    #region Editor

    [Serializable]
    public class BBTargetBindInspectorData: ShowInspectorData
    {
        private TargetBindInfo curInfo;
        
        [LabelText("当前帧: "),ReadOnly]
        public int currentFrame;

        [LabelText("当前位置: ")]
        public Vector2 curPos;
        
        [Button("保存",DirtyOnClick = false)]
        private void Save()
        {
            fieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                curInfo.TargetPosition = curPos;
            },"Save TargetBind Frame");
        }

        private TimelineFieldView fieldView;
        
        public BBTargetBindInspectorData(object target): base(target)
        {
            TargetBindInfo info = target as TargetBindInfo;
            curInfo = info;
            currentFrame = info.frame;
            curPos = info.TargetPosition;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            fieldView = _fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView _fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView _fieldView)
        {
        }
    }

    #endregion
}