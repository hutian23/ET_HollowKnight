using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEngine;

namespace Timeline
{
    [Serializable]
    [BBTrack("SubTimeline")]
#if UNITY_EDITOR
    [Color(127, 253, 228)]
    [IconGuid("799823b53d556d34faeb55e049c91845")]
#endif
    public class SubTimelineTrack: BBTrack
    {
#if UNITY_EDITOR
        public string referName;
#endif
        public string targetBindTrackName;
        public string SubTimelineName;
        public List<SubTimelineKeyFrame> KeyFrames = new();

        public SubTimelineKeyFrame GetKeyFrame(int targetFrame)
        {
            return KeyFrames.FirstOrDefault(keyFrame => keyFrame.frame == targetFrame);
        }
        
        public override Type RuntimeTrackType => typeof (RuntimeSubTimelineTrack);
        
#if UNITY_EDITOR
        public override Type TrackViewType => typeof (SubTimelineTrackView);  
#endif
    }

    [Serializable]
    public class SubTimelineKeyFrame: BBKeyframeBase
    {
        public int TimelineFrame;
    }
    
    #region Editor

    [Serializable]
    public class SubTimelineInspectorData: ShowInspectorData
    {
        [LabelText("当前帧: "), ReadOnly]
        public int CurFrame;
        
        // Track
        [LabelText("绑定轨道(TargetBind): "), PropertyOrder(0)]
        public string targetBindTrackName;
        [LabelText("绑定对象: "), PropertyOrder(1)]
        public GameObject referGo;
        [Button("绑定"),PropertyOrder(2)]
        public void Bind()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                Track.targetBindTrackName = targetBindTrackName;

                if (referGo == null)
                {
                    return;
                }
                ReferenceCollector refer = FieldView.EditorWindow.TimelinePlayer.GetComponent<ReferenceCollector>();
                if (refer.Get<GameObject>(refer.name) != null)
                {
                    refer.Add(refer.name, referGo);
                }
                Track.referName = referGo.name;
            }, "Rebind SubTimeline GameObject");
        }
        
        // KeyFrame
        // [LabelText("目标帧: "),Space(5), PropertyOrder(3)]
        // public int TimelineFrame;
        // [Button("保存"), PropertyOrder(5)]
        // public void Save()
        // {
        //     
        // }

        private readonly SubTimelineKeyFrame KeyFrame;
        private readonly SubTimelineTrack Track;
        private TimelineFieldView FieldView;
        
        public SubTimelineInspectorData(object target, SubTimelineTrack track): base(target)
        {
            KeyFrame = target as SubTimelineKeyFrame;
            Track = track;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            FieldView = _fieldView;
            CurFrame = KeyFrame.frame;
            targetBindTrackName = Track.targetBindTrackName;
            if (!string.IsNullOrEmpty(Track.referName))
            {
                ReferenceCollector refer = FieldView.EditorWindow.TimelinePlayer.GetComponent<ReferenceCollector>();
                referGo = refer.Get<GameObject>(Track.referName);
                if (referGo == null)
                {
                    Debug.LogError($"does not exist refer gameObject:{Track.referName}");
                }
            }
            // TimelineFrame = KeyFrame.TimelineFrame;
        }

        public override void InspectorUpdate(TimelineFieldView _fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView _fieldView)
        {
        }
    }
    
    #endregion
    
    #region Runtime
    
    public class RuntimeSubTimelineTrack : RuntimeTrack
    {
        public RuntimeSubTimelineTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
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
}