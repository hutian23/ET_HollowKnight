using System.Collections.Generic;
using Sirenix.OdinInspector;
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
        public int Hertz;
    }
    #endregion
    
    public sealed class TimelinePlayer: SerializedMonoBehaviour
    {
        [HideInInspector]
        public long instanceId; // timelineComponent.InstanceId
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

        // ReSharper disable once Unity.RedundantEventFunction
        private void OnAnimatorMove()
        {
            //禁用AnimationClip对transform的修改
        }
        
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
        
        public void Evaluate(int targetFrame)
        {
            RuntimePlayable.Evaluate(targetFrame);
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