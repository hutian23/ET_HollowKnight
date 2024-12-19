using System.Collections.Generic;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace Timeline
{
    #region Event

    public struct BehaviorControllerReloadCallback
    {
        public long instanceId;
    }
    
    public struct PreviewReloadCallback
    {
        public long instanceId;
        public BehaviorClip Clip;
    }

    public struct UpdateHertzCallback
    {
        public long instanceId;
        public float Hertz;
    }
    
    #endregion
    
    public sealed class TimelinePlayer: SerializedMonoBehaviour
    {
        [HideInInspector]
        public long instanceId; // TimelineComponent组件id
        [HideInInspector]
        public float Hertz = 60; //TimeScale
        public bool ApplyRootMotion;

        public bool IsValid => PlayableGraph.IsValid();
        private Animator Animator { get; set; }
        public PlayableGraph PlayableGraph { get; private set; }
        public AnimationLayerMixerPlayable AnimationRootPlayable { get; private set; }
        // private AudioMixerPlayable AudioRootPlayable { get; set; }

        [ShowIf("HasNotBindUnit")]
        public BBPlayableGraph BBPlayable;

        [HideInInspector]
        public BBTimeline CurrentTimeline;

        [HideInInspector]
        public RuntimePlayable RuntimePlayable;

        public void OnDisable()
        {
            Dispose();
        }

        [HideInInspector]
        //根运动的初始位置，跟animationCurve中的差值即为这一帧的移动距离
        public Vector3 initPos;

        public void OnEnable()
        {
            initPos = transform.localPosition;
        }

        // ReSharper disable once Unity.RedundantEventFunction
        private void OnAnimatorMove()
        {
            //禁用AnimationClip对transform的修改
        }

        private void OnGUI()
        {
            Debug.LogWarning("Hello World");
        }

#if UNITY_EDITOR
        [Button("技能编辑器"), ShowIf("HasNotBindUnit")]
        public void OpenWindow()
        {
            foreach (BBTimeline timeline in BBPlayable.timelineDict.Values)
            {
                OpenWindow(timeline);
                return;
            }
        }

        public void OpenWindow(BBTimeline timeline)
        {
            ClearTimelineGenerate();
            TimelineEditorWindow.OpenWindow(this, timeline);
        }

        [Button("清除运行时组件"), ShowIf("HasNotBindUnit")]
        public void ClearTimelineGenerate()
        {
            var goSet = new HashSet<GameObject>();
            foreach (var component in GetComponentsInChildren<Component>())
            {
                if (typeof(ITimelineGenerate).IsAssignableFrom(component.GetType()))
                {
                    goSet.Add(component.gameObject);
                }
            }

            foreach (GameObject go in goSet)
            {
                DestroyImmediate(go);
            }
        }

        public void ResetPosition()
        {
            transform.position = initPos;
        }
#endif

        public BBTimeline GetTimeline(string timelineName)
        {
            return BBPlayable.GetTimeline(timelineName);
        }
        
        public void Init(BBTimeline _timeline)
        {
            #region PlayableGraph

            PlayableGraph = PlayableGraph.Create(_timeline.timelineName);
            //混合
            AnimationRootPlayable = AnimationLayerMixerPlayable.Create(PlayableGraph);
            Animator = GetComponent<Animator>();
            AnimationPlayableOutput playableOutput = AnimationPlayableOutput.Create(PlayableGraph, "Animation", Animator);
            playableOutput.SetSourcePlayable(AnimationRootPlayable);

            #endregion

            #region RuntimeTimeline

            CurrentTimeline = _timeline;
            RuntimePlayable = RuntimePlayable.Create(CurrentTimeline, this);
            #endregion
        }
        
        

        public void Dispose()
        {
            if (PlayableGraph.IsValid()) PlayableGraph.Destroy();
        }

        /// <summary>
        /// 运行时 逻辑层传回组件instanceId给loader层回调事件
        /// </summary>
        /// <returns></returns>
        public bool HasBindUnit
        {
            get
            {
                return instanceId != 0;
            }
        }

        public bool HasNotBindUnit
        {
            get => !HasBindUnit;
        }
    }
}