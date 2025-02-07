using System;
using System.Collections.Generic;
using System.Linq;
using ET;
using Sirenix.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using Object = UnityEngine.Object;

namespace Timeline
{
    [Serializable]
    [BBTrack("TargetBind")]
    [Color(100, 100, 100)]
    [IconGuid("51d6e4824d3138c4880ca6308fa0e473")]
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
        
        public string referName;
        
        public override int GetMaxFrame()
        {
            return KeyFrames.Count > 0 ? KeyFrames.Max(info => info.frame) : 0;
        }
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

    public struct UpdateTargetBindCallback
    {
        public long instanceId;
        public TargetBindKeyFrame KeyFrame;
        public BBTargetBindTrack BindTrack;
    }
    
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
#if UNITY_EDITOR
            if (TimelinePlayer.HasBindUnit)
            {
                return;
            }
            GenerateTargetBind();
            EditorApplication.update += SyncPosition;      
#endif
        }

        public override void UnBind()
        {
#if UNITY_EDITOR
            if (TimelinePlayer.HasBindUnit)
            {
                return;
            }
            Object.DestroyImmediate(TargetBindGo);
            EditorApplication.update -= SyncPosition;   
#endif
        }
        
        public override void SetTime(int targetFrame)
        {
            if (TimelinePlayer.HasBindUnit)
            {
                TargetBindKeyFrame _keyFrame = BindTrack.GetKeyFrame(targetFrame);
                if (_keyFrame == null)
                {
                    return;
                }
                EventSystem.Instance.Invoke(new UpdateTargetBindCallback(){ instanceId = TimelinePlayer.instanceId, KeyFrame = _keyFrame, BindTrack = BindTrack});
            }
            else
            {
#if UNITY_EDITOR
                TargetBindKeyFrame _keyFrame = BindTrack.GetClosestKeyFrame(targetFrame);
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
#endif
            }
        }

        private void SyncPosition()
        {
            // 运行时退出可能并没有销毁委托，导致空引用
            if (TimelinePlayer == null)
            {
#if UNITY_EDITOR
                EditorApplication.update -= SyncPosition;          
#endif
            }
            
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
        
        private void GenerateTargetBind()
        {
            //Generate TargetBind GameObject
            TargetBindGo = new(BindTrack.Name);
            TargetBindGo.transform.SetParent(TimelinePlayer.transform);
            TargetBindGo.transform.localPosition = Vector3.zero;
            TargetBindGo.AddComponent<TargetBind>().TrackName = BindTrack.Name;
        }
        
    }
}