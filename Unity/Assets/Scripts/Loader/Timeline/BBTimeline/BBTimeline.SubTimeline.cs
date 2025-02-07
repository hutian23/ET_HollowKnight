using System;
using System.Collections.Generic;
using System.Linq;
using ET;
using UnityEngine;

namespace Timeline
{
    [Serializable]
    [BBTrack("SubTimeline")]
    [Color(127, 253, 228)]
    [IconGuid("799823b53d556d34faeb55e049c91845")]
    public class SubTimelineTrack: BBTrack
    {
        public List<SubTimelineKeyFrame> KeyFrames = new();
        
        public SubTimelineKeyFrame GetKeyFrame(int targetFrame)
        {
            return KeyFrames.FirstOrDefault(keyFrame => keyFrame.frame == targetFrame);
        }

        public SubTimelineKeyFrame GetClosestKeyFrame(int targetFrame)
        {
            int closestFrame = -1;
            foreach (SubTimelineKeyFrame keyFrame in KeyFrames)
            {
                if (keyFrame.frame == targetFrame)
                {
                    closestFrame = keyFrame.frame;
                    break;
                }
                if (keyFrame.frame < targetFrame && targetFrame - keyFrame.frame < targetFrame - closestFrame)
                {
                    closestFrame = keyFrame.frame;
                }
            }
            return closestFrame == -1 ? null : GetKeyFrame(closestFrame);
        }
        
        public override Type RuntimeTrackType => typeof (RuntimeSubTimelineTrack);
        
#if UNITY_EDITOR
        public string referName;
        public BBTimeline subTimeline;
#endif
    }

    [Serializable]
    public class SubTimelineKeyFrame: BBKeyframeBase
    {
        public int TimelineFrame;
    }
    
    #region Runtime

    public struct UpdateSubTimelineCallback
    {
        public long instanceId;
        public SubTimelineTrack Track;
        public SubTimelineKeyFrame keyFrame;
    }
    
    public class RuntimeSubTimelineTrack : RuntimeTrack
    {
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;
        private SubTimelineTrack subTimelineTrack => Track as SubTimelineTrack;
        
        public RuntimeSubTimelineTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
#if UNITY_EDITOR
            if (timelinePlayer.HasBindUnit)
            {
                return;
            }
            
            ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
            GameObject referGo = refer.Get<GameObject>(subTimelineTrack.referName);
            if (referGo == null)
            {
                return;
            }
            
            TimelinePlayer subTimelinePlayer = referGo.GetComponent<TimelinePlayer>();
            subTimelinePlayer.Init(subTimelineTrack.subTimeline);      
#endif
        }

        public override void UnBind()
        {
        }

        public override void SetTime(int targetFrame)
        {
            if (timelinePlayer.HasBindUnit)
            {
                SubTimelineKeyFrame _keyFrame = subTimelineTrack.GetKeyFrame(targetFrame);
                if (_keyFrame == null)
                {
                    return;
                }
                EventSystem.Instance.Invoke(new UpdateSubTimelineCallback(){instanceId = timelinePlayer.instanceId, keyFrame = _keyFrame, Track = subTimelineTrack});
            }
            else
            {
#if UNITY_EDITOR
                //1. Find TimelinePlayer
                ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
                GameObject referGo = refer.Get<GameObject>(subTimelineTrack.referName);
                if (referGo == null)
                {
                    return;
                }
                TimelinePlayer subTimelinePlayer = referGo.GetComponent<TimelinePlayer>();
            
                //2. Find current KeyFrame
                SubTimelineKeyFrame _keyFrame = subTimelineTrack.GetClosestKeyFrame(targetFrame);
                if (_keyFrame == null)
                {
                    return;
                }
                subTimelinePlayer.Evaluate(_keyFrame.TimelineFrame);          
#endif
            }
        }
    }
    
    #endregion
}