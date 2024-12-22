﻿using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline.Editor;
using UnityEngine;
using EventSystem = ET.EventSystem;

namespace Timeline
{
    [Serializable]
    [BBTrack("Event")]
#if UNITY_EDITOR
    [Color(100, 100, 100)]
    [IconGuid("423519931448cce4b8c8320e4f526f3b")]
#endif
    public class BBEventTrack: BBTrack
    {
        [NonSerialized, OdinSerialize]
        public List<EventInfo> EventInfos = new();

        public override Type RuntimeTrackType => typeof (RuntimeEventTrack);

        public EventInfo GetInfo(int targetFrame)
        {
            return EventInfos.FirstOrDefault(info => info.frame == targetFrame);
        }

        public EventInfo GetInfo(string markerName)
        {
            foreach (EventInfo info in EventInfos)
            {
                if (info.keyframeName.Equals(markerName))
                {
                    return info;
                }
            }
            
            Debug.LogError($"does not exist eventInfo for {markerName}");
            return null;
        }
        
#if UNITY_EDITOR
        public override Type TrackViewType => typeof (EventTrackView);
        public override int GetMaxFrame()
        {
            return EventInfos.Count > 0? EventInfos.Max(info => info.frame) : 0;
        }
#endif
    }

    [Serializable]
    public class EventInfo: BBKeyframeBase
    {
    }

#if UNITY_EDITOR
    public class EventInspectorData: ShowInspectorData
    {
        private TimelineFieldView FieldView;

        [HideReferenceObjectPicker, HideLabel]
        public EventInfo Info;

        [Button("保存")]
        public void Save()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() => { }, "Save info", false);
        }

        public EventInspectorData(object target): base(target)
        {
            Info = target as EventInfo;
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
        }
    }
#endif

    public struct UpdateEventTrackCallback
    {
        public long instanceId;
        public string markerName;
    }
    
    public class RuntimeEventTrack: RuntimeTrack
    {
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;

        public RuntimeEventTrack(RuntimePlayable runtimePlayable, BBTrack _track): base(runtimePlayable, _track)
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
            if (!timelinePlayer.HasBindUnit)
            {
                return;
            }
            
            BBEventTrack eventTrack = Track as BBEventTrack;
            EventInfo info = eventTrack.GetInfo(targetFrame);
            if (info == null)
            {
                return;
            }
            EventSystem.Instance?.Invoke(new UpdateEventTrackCallback() { instanceId = timelinePlayer.instanceId, markerName = info.keyframeName});
        }
    }
}