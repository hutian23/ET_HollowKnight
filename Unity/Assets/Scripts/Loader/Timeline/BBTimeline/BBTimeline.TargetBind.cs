using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline.Editor;
using UnityEngine;
using Object = UnityEngine.Object;

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
            return KeyFrames.FirstOrDefault(info => info.frame == targetFrame);
        }
        
#if UNITY_EDITOR
        public override Type TrackViewType => typeof (TargetBindTrackView);

        public override int GetMaxFrame()
        {
            return KeyFrames.Count > 0 ? KeyFrames.Max(info => info.frame) : 0;
        }
#endif
    }

    [Serializable]
    public class TargetBindKeyFrame: BBKeyframeBase
    {
        public Vector2 LocalPosition;
        public FlipState Flip = FlipState.Left;
    }
    
    [Flags]
    public enum FlipState
    {
        Left = 1,
        Right = -1
    }
    
    #region Runtime

    public class RuntimeTargetBindTrack: RuntimeTrack
    {
        private TimelinePlayer TimelinePlayer => RuntimePlayable.TimelinePlayer;
        private readonly BBTargetBindTrack BindTrack;
        private GameObject TargetBindGo;
        
        public RuntimeTargetBindTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
           BindTrack = Track as BBTargetBindTrack;
        }

        public override void Bind()
        {
            GenerateTargetBind();
        }

        public override void UnBind()
        {
            Object.DestroyImmediate(TargetBindGo);
        }

        public override void SetTime(int targetFrame)
        {
            foreach (TargetBindKeyFrame keyFrame in BindTrack.KeyFrames)
            {
                if (keyFrame.frame != targetFrame)
                {
                    continue;
                }
                if (TargetBindGo == null)
                {
                    GenerateTargetBind();
                }
            }
        }

        private void GenerateTargetBind()
        {
            //Generate TargetBind GameObject
            TargetBindGo = new(BindTrack.Name);
            TargetBindGo.transform.SetParent(TimelinePlayer.transform);
            TargetBindGo.transform.localPosition = Vector3.zero;
            TargetBindGo.AddComponent<TargetBind>().TrackName = BindTrack.Name;
        }
    }

    #endregion

    #region Editor
    [Serializable]
    public class BBTargetBindInspectorData: ShowInspectorData
    {
        private TargetBindKeyFrame CurKeyFrame;
        private BBTargetBindTrack BindTrack;
        private TimelineFieldView FieldView;
        
        [LabelText("当前帧: "), ReadOnly]
        public int CurFrame;
        
        [LabelText("相对位置: "), ReadOnly]
        public Vector2 LocalPos;

        [LabelText("转向: ")]
        public FlipState Flip;
        
        [Button("保存",DirtyOnClick = false)]
        private void Save()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                CurKeyFrame.Flip = Flip;
                //保存相对位置
                foreach (TargetBind targetBind in FieldView.EditorWindow.TimelinePlayer.GetComponentsInChildren<TargetBind>())
                {
                    if (targetBind.TrackName.Equals(BindTrack.Name))
                    {
                        GameObject targetGo = targetBind.gameObject;
                        CurKeyFrame.LocalPosition = targetGo.transform.localPosition;
                        return;
                    }
                }
                Debug.LogError($"Does not exist targetBind GameObject: {BindTrack.Name}");
                
            }, "Save TargetBind KeyFrame");
        }
        
        public BBTargetBindInspectorData(object target, BBTargetBindTrack bindTrack): base(target)
        {
            TargetBindKeyFrame keyFrame = target as TargetBindKeyFrame; 
            CurKeyFrame = keyFrame;
            BindTrack = bindTrack;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            FieldView = _fieldView;
            CurFrame = CurKeyFrame.frame;
            LocalPos = CurKeyFrame.LocalPosition;
            Flip = CurKeyFrame.Flip;
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