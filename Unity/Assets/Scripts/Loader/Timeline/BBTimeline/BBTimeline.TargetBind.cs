using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline.Editor;
using UnityEditor;
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

        public TargetBindKeyFrame GetKeyFrame(int targetFrame)
        {
            return KeyFrames.FirstOrDefault(info => info.frame == targetFrame);
        }

        // 当前帧号是否存在关键帧，不存在则往时间轴左边取最近的关键帧 编辑器模式下可能会往回拉
        public TargetBindKeyFrame GetClosestKeyFrame(int targetFrame)
        {
            int closestFrame = -1;
            foreach (TargetBindKeyFrame keyFrame in KeyFrames)
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
        
#if UNITY_EDITOR
        public override Type TrackViewType => typeof (TargetBindTrackView);
        public string referName;
        
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
            EditorApplication.update += SyncPosition;
        }

        public override void UnBind()
        {
            Object.DestroyImmediate(TargetBindGo);
            EditorApplication.update -= SyncPosition;
        }

        private void SyncPosition()
        {
            //1. Find refer GameObject
            ReferenceCollector refer = TimelinePlayer.GetComponent<ReferenceCollector>();
            GameObject referGo = refer.Get<GameObject>(BindTrack.referName);
            if (referGo == null || TargetBindGo == null)
            {
                return;
            }
            //2. Sync Position
            referGo.transform.position = TargetBindGo.transform.position;
            //3. Sync Flip
            TargetBind targetBind = TargetBindGo.GetComponent<TargetBind>();
            referGo.transform.localScale = new Vector3((int)targetBind.curFlip, 1, 1);
        }
        
        public override void SetTime(int targetFrame)
        {
            TargetBindKeyFrame _keyFrame = TimelinePlayer.HasBindUnit? BindTrack.GetKeyFrame(targetFrame) : BindTrack.GetClosestKeyFrame(targetFrame);
            if (_keyFrame == null)
            {
                return;
            }
            if (TargetBindGo == null)
            {
                GenerateTargetBind();
            }
            // 更新TargetBindGo位置
            TargetBindGo.transform.localPosition = _keyFrame.LocalPosition;
            // 更新转向
            TargetBind targetBind = TargetBindGo.GetComponent<TargetBind>();
            targetBind.curFlip = _keyFrame.Flip;
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
        private TargetBindKeyFrame KeyFrame;
        private BBTargetBindTrack BindTrack;
        private TimelineFieldView FieldView;
        
        [LabelText("当前帧: "), ReadOnly]
        public int CurFrame;
        
        [LabelText("绑定对象")]
        public GameObject TargetBindGo;
        
        [LabelText("相对位置: "), ReadOnly]
        public Vector2 LocalPos;

        [LabelText("转向: ")]
        public FlipState Flip;
        
        [Button("保存",DirtyOnClick = false)]
        private void Save()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                //保存绑定对象
                ReferenceCollector refer = FieldView.EditorWindow.TimelinePlayer.GetComponent<ReferenceCollector>();
                refer.Remove(TargetBindGo.name);
                refer.Add(TargetBindGo.name, TargetBindGo);
                BindTrack.referName = TargetBindGo.name;
                
                //保存转向
                KeyFrame.Flip = Flip;
                
                //保存相对位置
                foreach (TargetBind targetBind in FieldView.EditorWindow.TimelinePlayer.GetComponentsInChildren<TargetBind>())
                {
                    if (targetBind.TrackName.Equals(BindTrack.Name))
                    {
                        GameObject targetGo = targetBind.gameObject;
                        KeyFrame.LocalPosition = targetGo.transform.localPosition;
                        return;
                    }
                }
                Debug.LogError($"Does not exist targetBind GameObject: {BindTrack.Name}");
                
            }, "Save TargetBind KeyFrame");
        }
        
        public BBTargetBindInspectorData(object target, BBTargetBindTrack bindTrack): base(target)
        {
            TargetBindKeyFrame keyFrame = target as TargetBindKeyFrame; 
            KeyFrame = keyFrame;
            BindTrack = bindTrack;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            FieldView = _fieldView;
            CurFrame = KeyFrame.frame;
            LocalPos = KeyFrame.LocalPosition;
            Flip = KeyFrame.Flip;

            if (string.IsNullOrEmpty(BindTrack.referName)) return;
            ReferenceCollector refer = FieldView.EditorWindow.TimelinePlayer.GetComponent<ReferenceCollector>();
            TargetBindGo = refer.Get<GameObject>(BindTrack.referName);
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