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
        public List<TargetBindKeyFrame> KeyFrames = new();
        
        public override Type RuntimeTrackType => typeof (RuntimeTargetBindTrack);

        public TargetBindKeyFrame GetInfo(int targetFrame)
        {
            return this.KeyFrames.FirstOrDefault(info => info.frame == targetFrame);
        }
        
#if UNITY_EDITOR
        public override Type TrackViewType => typeof (TargetBindTrackView);

        public override int GetMaxFrame()
        {
            return this.KeyFrames.Count > 0 ? this.KeyFrames.Max(info => info.frame) : 0;
        }
#endif
    }

    [Serializable]
    public class TargetBindKeyFrame: BBKeyframeBase
    {
        public Vector2 TargetPosition;
    }
    
    #region Runtime

    public class RuntimeTargetBindTrack: RuntimeTrack
    {
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;
        private readonly BBTargetBindTrack targetBindTrack;
        private int currentFrame = -1;
        
        public RuntimeTargetBindTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
           targetBindTrack = Track as BBTargetBindTrack;
        }

        public override void Bind()
        {
            GameObject child = new(targetBindTrack.Name);
            child.transform.SetParent(timelinePlayer.transform);
            child.transform.localPosition = Vector2.zero;
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
        private TargetBindKeyFrame curKeyFrame;
        
        [LabelText("当前帧: "),ReadOnly]
        public int currentFrame;

        [LabelText("当前位置: ")]
        public Vector2 curPos;
        
        [Button("保存",DirtyOnClick = false)]
        private void Save()
        {
            fieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                this.curKeyFrame.TargetPosition = curPos;
            },"Save TargetBind Frame");
        }

        private TimelineFieldView fieldView;
        
        public BBTargetBindInspectorData(object target): base(target)
        {
            TargetBindKeyFrame keyFrame = target as TargetBindKeyFrame;
            this.curKeyFrame = keyFrame;
            currentFrame = keyFrame.frame;
            curPos = keyFrame.TargetPosition;
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