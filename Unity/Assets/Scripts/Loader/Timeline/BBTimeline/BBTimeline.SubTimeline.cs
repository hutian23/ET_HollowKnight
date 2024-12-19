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
        public List<SubTimelineKeyFrame> KeyFrames = new();

        public SubTimelineKeyFrame GetKeyFrame(int targetFrame)
        {
            return KeyFrames.FirstOrDefault(keyFrame => keyFrame.frame == targetFrame);
        }
        
        public override Type RuntimeTrackType => typeof (RuntimeSubTimelineTrack);
        
#if UNITY_EDITOR
        public override Type TrackViewType => typeof (SubTimelineTrackView);  
        public string referName;
        public BBTimeline subTimeline;
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
        [LabelText("当前帧: "), ReadOnly, PropertyOrder(0)]
        public int curFrame;
        
        [LabelText("子对象: "), PropertyOrder(1)]
        public GameObject referGo;

        [LabelText("子行为: ")]
        public BBTimeline subTimeline;
        
        [LabelText("目标帧: "), PropertyOrder(3)]
        public int timelineFrame;
        
        [Button("保存"), PropertyOrder(4)]
        public void Save()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                KeyFrame.TimelineFrame = this.timelineFrame;
                
                if (referGo == null)
                {
                    Track.referName = string.Empty;
                }
                // 因为GameObject不能跨场景保存，因此仅保存引用
                else
                {
                    ReferenceCollector refer = FieldView.EditorWindow.TimelinePlayer.GetComponent<ReferenceCollector>();
                    refer.Remove(referGo.name);
                    refer.Add(referGo.name,referGo);
                    Track.referName = referGo.name;
                }

                Track.subTimeline = subTimeline;
            },"Save SubTimeline KeyFrame");
        }

        [Button("预览",DirtyOnClick = false), PropertyOrder(5)]
        public void PreView()
        {
            TimelinePlayer referPlayer = referGo.GetComponent<TimelinePlayer>();
            referPlayer.Init(subTimeline);
            referPlayer.Evaluate(timelineFrame);
        }

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
            curFrame = KeyFrame.frame;
            
            timelineFrame = KeyFrame.TimelineFrame;
            subTimeline = Track.subTimeline;
            
            //Find Refer GameObject
            ReferenceCollector refer = FieldView.EditorWindow.TimelinePlayer.GetComponent<ReferenceCollector>();
            referGo = refer.Get<GameObject>(Track.referName);
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
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;
        
        public RuntimeSubTimelineTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
            SubTimelineTrack subTimelineTrack = Track as SubTimelineTrack;
            
            ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
            GameObject referGo = refer.Get<GameObject>(subTimelineTrack.referName);
            if (referGo == null)
            {
                return;
            }
            
            TimelinePlayer subTimelinePlayer = referGo.GetComponent<TimelinePlayer>();
            subTimelinePlayer.Init(subTimelineTrack.subTimeline);
        }

        public override void UnBind()
        {
        }

        public override void SetTime(int targetFrame)
        {
            SubTimelineTrack subTimelineTrack = Track as SubTimelineTrack;
         
            //1. Find TimelinePlayer
            ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
            GameObject referGo = refer.Get<GameObject>(subTimelineTrack.referName);
            if (referGo == null)
            {
                return;
            }
            TimelinePlayer subTimelinePlayer = referGo.GetComponent<TimelinePlayer>();
            
            //2. Find current KeyFrame
            foreach (SubTimelineKeyFrame keyFrame in subTimelineTrack.KeyFrames)
            {
                if (keyFrame.frame != targetFrame)
                {
                    continue;
                }

                subTimelinePlayer.Evaluate(keyFrame.TimelineFrame);
                break;
            }
        }
    }
    
    #endregion
}