﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Audio;
using UnityEngine.Playables;

namespace ET
{
    public partial class Timeline: ScriptableObject
    {
        [SerializeReference]
        protected List<Track> m_Tracks = new();

        public List<Track> Tracks => this.m_Tracks;

        public event Action OnEvaluated;
        public event Action OnRebind;
        public event Action OnDone;
        public event Action OnBindStateChanged;
        public event Action OnValueChanged;

        private float m_Time;

        public float Time
        {
            get => this.m_Time;
            set
            {
                this.m_Time = value;
                this.OnEvaluated?.Invoke();
            }
        }

        public int Frame => Mathf.RoundToInt(this.Time * TimelineUtility.FrameRate);
        public int MaxFrame { get; protected set; }
        public float Duration { get; protected set; }

        private bool m_Binding;

        public bool Binding
        {
            get => this.m_Binding;
            protected set => this.m_Binding = value;
        }

        private TimelinePlayer m_TimelinePlayer;

        public TimelinePlayer TimelinePlayer
        {
            get => this.m_TimelinePlayer;
            protected set => this.m_TimelinePlayer = value;
        }

        public PlayableGraph PlayableGraph { get; protected set; }
        public AnimationLayerMixerPlayable AnimationRootPlayable { get; protected set; }
        public AudioMixerPlayable AudioRootPlayable { get; protected set; }

        public void Init()
        {
            #region UnBind

            bool isBinding = this.Binding;
            TimelinePlayer timelinePlayer = TimelinePlayer;
            if (isBinding)
            {
                timelinePlayer.Dispose();
            }

            #endregion

            #region Init

            m_Tracks.ForEach(t => t.Init(this));
            MaxFrame = 0;
            foreach (var track in m_Tracks)
            {
                track.Init(this);
                if (track.MaxFrame > MaxFrame)
                {
                    MaxFrame = track.MaxFrame;
                }
            }

            Duration = (float)MaxFrame / TimelineUtility.FrameRate;
            OnValueChanged?.Invoke();

            #endregion

            #region Bind

            if (isBinding)
            {
                timelinePlayer.Init();
                timelinePlayer.AddTimeline(this);
            }

            #endregion
        }

        public void Evaluate(float deltaTime)
        {
            Time += deltaTime;
            Tracks.ForEach(t => t.Evaluate(deltaTime));
            if (Time > Duration)
            {
                OnDone?.Invoke();
                OnDone = null;
            }
        }

        public void Bind(TimelinePlayer timelinePlayer)
        {
            this.Time = 0;
            this.TimelinePlayer = timelinePlayer;
            this.PlayableGraph = timelinePlayer.PlayableGraph;
            this.AnimationRootPlayable = timelinePlayer.AnimationRootPlayable;
            this.AudioRootPlayable = timelinePlayer.AudioRootPlayable;

            this.Binding = true;
            this.OnRebind = null;
            this.OnValueChanged += this.RebindAll;

            this.m_Tracks.ForEach(t => t.Bind());
            this.OnBindStateChanged?.Invoke();
        }

        public void UnBind()
        {
            this.m_Tracks.ForEach(t => t.UnBind());
            this.Binding = false;
            this.OnRebind = null;
            this.OnValueChanged -= this.RebindAll;

            this.AnimationRootPlayable = AnimationLayerMixerPlayable.Null;
            this.AudioRootPlayable = default;
            this.PlayableGraph = default;
            this.TimelinePlayer = null;

            this.OnBindStateChanged?.Invoke();
        }

        public void JumpTo(float targetTime)
        {
            float deltaTime = targetTime - Time;
            TimelinePlayer.AddtionalDelta = deltaTime;
        }

        public void RebindAll()
        {
            if (this.Binding)
            {
                this.OnRebind?.Invoke();
                this.OnRebind = null;

                foreach (var track in this.m_Tracks)
                {
                    track.ReBind();
                    track.SetTime(this.Time);
                }

                TimelinePlayer.Evaluate(0);
            }
        }

        public void RebindTrack(Track track)
        {
            if (this.Binding)
            {
                track.ReBind();
                track.SetTime(this.Time);
                this.TimelinePlayer.Evaluate(0);
            }
        }

        public void RuntimeMute(int index, bool value)
        {
            if (0 <= index && index < this.m_Tracks.Count)
            {
                RuntimeMute(this.m_Tracks[index], value);
            }
        }

        public void RuntimeMute(Track track, bool value)
        {
            track.RuntimeMute(value);
        }
    }

    [Serializable]
    public abstract partial class Track
    {
        public string Name;

        [SerializeField]
        protected bool m_PersistentMuted; // 持续

        public bool PersistentMuted
        {
            get => m_PersistentMuted;
            set
            {
                if (m_PersistentMuted != value)
                {
                    m_PersistentMuted = value;
                    OnMutedStateChanged?.Invoke();
                }
            }
        }

        protected bool m_RuntimeMuted;

        public bool RuntimeMuted
        {
            get => m_RuntimeMuted;
            set
            {
                if (m_RuntimeMuted != value)
                {
                    m_RuntimeMuted = value;
                    OnMutedStateChanged?.Invoke();
                }
            }
        }

        [SerializeReference]
        protected List<Clip> m_Clips = new();

        public List<Clip> Clips => m_Clips;

        public Action OnUpdateMix;
        public Action OnMutedStateChanged;
        public Timeline Timeline { get; protected set; }
        public int MaxFrame { get; protected set; }

        public virtual void Init(Timeline timeline)
        {
            this.Timeline = timeline;
            this.MaxFrame = 0;
            foreach (var clip in m_Clips)
            {
                clip.Init(this);
                if (clip.EndFrame > MaxFrame)
                {
                    MaxFrame = clip.EndFrame;
                }
            }

            this.RuntimeMuted = false;
        }

        public virtual void Bind()
        {
            m_Clips.ForEach(c => c.Bind());
        }

        public virtual void UnBind()
        {
            m_Clips.ForEach(c => c.UnBind());
        }

        public virtual void ReBind()
        {
            this.UnBind();
            this.Bind();
        }

        public virtual void Evaluate(float deltaTime)
        {
            if (this.m_PersistentMuted || this.m_RuntimeMuted)
            {
                return;
            }

            m_Clips.ForEach(c => c.Evaluate(deltaTime));
        }

        public virtual void SetTime(float time)
        {
            if (this.m_PersistentMuted || this.m_RuntimeMuted)
            {
                return;
            }

            this.m_Clips.ForEach(c => c.Evaluate(time));
        }

        public virtual void RuntimeMute(bool value)
        {
            if (this.PersistentMuted)
            {
                return;
            }

            if (value && !this.RuntimeMuted)
            {
                this.RuntimeMuted = true;
                this.UnBind();
            }
            else if (!value && this.RuntimeMuted)
            {
                RuntimeMuted = false;
                Bind();
                SetTime(Timeline.Time);
            }
        }
    }

    [Serializable]
    public abstract partial class Clip
    {
        #region Frame

        public int StartFrame;
        public int EndFrame;
        public int OtherEaseInFrame;
        public int OtherEaseOutFrame;
        public int SelfEaseInFrame;
        public int SelfEaseOutFrame;
        public int ClipInFrame;

        public int EaseInFrame => OtherEaseInFrame == 0? SelfEaseInFrame : OtherEaseInFrame;
        public int EaseOutFrame => OtherEaseOutFrame == 0? SelfEaseOutFrame : OtherEaseOutFrame;
        public int Duration => this.EndFrame - this.StartFrame;

        #endregion

        #region Time

        public float StartTime { get; private set; }
        public float EndTime { get; private set; }
        public float OtherEaseInTime { get; private set; }
        public float OtherEaseOutTime { get; private set; }
        public float EaseInTime { get; private set; }
        public float EaseOutTime { get; private set; }
        public float ClipInTime { get; private set; }
        public float DurationTime { get; private set; }

        #endregion

        public bool CanSkip;

        [NonSerialized]
        public Track Track;

        public Timeline Timeline => this.Track.Timeline;
        public bool Active { get; protected set; }
        public float Time { get; protected set; }
        public float TargetTime { get; protected set; }
        public float OffsetTime => Time - StartTime + ClipInTime;

        public Action OnNameChanged;
        public Action OnInspectorRepaint;

        public virtual void Init(Track track)
        {
            this.Track = track;
        }

        public virtual void Bind()
        {
            Active = false;
            Time = 0;
        }

        public virtual void UnBind()
        {
            if (Active)
            {
                OnDisable();
            }

            Active = false;
            Time = 0;
        }

        public virtual void Evaluate(float deltaTime)
        {
            TargetTime = Time + deltaTime;
            if (!Active && StartTime <= TargetTime && TargetTime <= EndTime)
            {
                Active = true;
                OnEnable();
            }
            else if (Active && (TargetTime < StartTime || EndTime < TargetTime))
            {
                Active = false;
                OnDisable();
            }

            if (!CanSkip)
            {
                if (!Active && Time < StartTime && EndTime < TargetTime)
                {
                    Active = true;
                    OnEnable();
                    Active = false;
                    OnDisable();
                }
                else if (!Active && TargetTime < StartTime && EndTime < Time)
                {
                    Active = true;
                    OnEnable();
                    Active = false;
                    OnDisable();
                }
            }

            Time = TargetTime;
        }

        public virtual void OnEnable()
        {
        }

        public virtual void OnDisable()
        {
        }

        public void FrameToTime()
        {
            StartTime = StartFrame / (float)TimelineUtility.FrameRate;
            EndTime = EndFrame / (float)TimelineUtility.FrameRate;
            OtherEaseInTime = OtherEaseInFrame / (float)TimelineUtility.FrameRate;
            OtherEaseOutTime = OtherEaseOutFrame / (float)TimelineUtility.FrameRate;
            EaseInTime = EaseInFrame / (float)TimelineUtility.FrameRate;
            EaseOutTime = EaseOutFrame / (float)TimelineUtility.FrameRate;
            ClipInTime = ClipInFrame / (float)TimelineUtility.FrameRate;
            DurationTime = Duration / (float)TimelineUtility.FrameRate;
        }
    }

    public abstract partial class SignalClip: Clip
    {
        public override void Evaluate(float deltaTime)
        {
            TargetTime = Time + deltaTime;

            if (!Active && StartTime <= TargetTime)
            {
                Active = true;
                OnEnable();
            }
            else if(Active && TargetTime < StartTime)
            {
                Active = false;
                OnDisable();
            }

            Time = TargetTime;
        }
    }
    
#if UNITY_EDITOR
    public partial class Timeline
    {
        public float Scale = 1;
        public UnityEditor.SerializedObject SerializedTimeline;

        public void AddTrack(Type type)
        {
            Track track = Activator.CreateInstance(type) as Track;
            track.Name = type.Name.Replace("Track", string.Empty);
            m_Tracks.Add(track);
            Init();
        }

        public void RemoveTrack(Track track)
        {
            m_Tracks.Remove(track);
            Init();
        }

        public Clip AddClip(Track track, int frame)
        {
            Clip clip = track.AddClip(frame);
            Init();
            return clip;
        }

        public Clip AddClip(UnityEngine.Object referenceObject, Track track, int frame)
        {
            Clip clip = track.AddClip(referenceObject, frame);
            Init();
            return clip;
        }

        public void RemoveClip(Clip clip)
        {
            clip.Track.RemoveClip(clip);
            Init();
        }

        public void UpdateMix()
        {
            m_Tracks.ForEach(track => track.UpdateMix());
        }

        public void Resort()
        {
            OnValueChanged?.Invoke();
        }

        public void ApplyModify(Action action, string _name)
        {
            UnityEditor.Undo.RegisterCompleteObjectUndo(this, $"Timeline: {_name}");
            SerializedTimeline.Update();
            action?.Invoke();
            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void UpdateSerializedTimeline()
        {
            SerializedTimeline = new UnityEditor.SerializedObject(this);
        }

        [UnityEditor.MenuItem("Assets/Create/ScriptableObject/Timeline/Timeline")]
        public static void CreateTimeline()
        {
            Timeline timeline = CreateInstance<Timeline>();
            string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
            string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path + "/New Timeline.asset");
            UnityEditor.AssetDatabase.CreateAsset(timeline,assetPathAndName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            UnityEditor.Selection.activeObject = timeline;
        }
    }

    public abstract partial class Track
    {
        //AudioTrack ---> AudioClip
        public virtual Type ClipType => typeof (Clip);

        public virtual Clip AddClip(int frame)
        {
            Clip clip = Activator.CreateInstance(ClipType, this, frame) as Clip;
            m_Clips.Add(clip);
            return clip;
        }

        public virtual Clip AddClip(UnityEngine.Object referenceObject, int frame)
        {
            return null;
        }

        public void RemoveClip(Clip clip)
        {
            m_Clips.Remove(clip);
        }

        public void UpdateMix()
        {
            this.Clips.ForEach(c =>
            {
                c.UpdateMix();
                c.FrameToTime();
            });
        }

        public Color Color()
        {
            var colorAttributes = GetType().GetCustomAttributes<ColorAttribute>().ToArray();
            return colorAttributes[^1].Color / 255;
        }

        public virtual bool DragValid()
        {
            return false;
        }

        public void RebindTimeline()
        {
            Timeline.RebindTrack(this);
        }
    }

    public abstract partial class Clip
    {
        [NonSerialized]
        public bool Invalid;

        public virtual string Name => GetType().Name;
        public virtual int Length => EndFrame - StartFrame;
        public virtual ClipCapabilities Capabilities => ClipCapabilities.None;

        public Clip()
        {
        }

        public Clip(Track track, int frame)
        {
            Track = track;
            StartFrame = frame;
            EndFrame = this.StartFrame + 3;
        }

        public void UpdateMix()
        {
            OtherEaseInFrame = 0;
            OtherEaseOutFrame = 0;

            if (Invalid)
            {
                return;
            }

            foreach (var clip in Track.Clips)
            {
                if (clip != this && !clip.Invalid)
                {
                    //包含
                    if (clip.StartFrame < StartFrame && clip.EndFrame > EndFrame)
                    {
                        return;
                    }
                    //被包含
                    else if (clip.StartFrame > StartFrame && clip.EndFrame < EndFrame)
                    {
                        return;
                    }

                    //在当前段前面
                    if (clip.StartFrame < StartFrame && clip.EndFrame > StartFrame)
                    {
                        OtherEaseInFrame = clip.EndFrame - StartFrame;
                    }

                    if (clip.StartFrame == StartFrame)
                    {
                        if (clip.EndFrame < EndFrame)
                        {
                            OtherEaseInFrame = clip.EndFrame - StartFrame;
                        }
                        else if (clip.EndFrame > EndFrame)
                        {
                            OtherEaseOutFrame = EndFrame - StartFrame;
                        }
                    }

                    SelfEaseInFrame = Mathf.Min(SelfEaseInFrame, Duration - OtherEaseOutFrame);
                    SelfEaseOutFrame = Mathf.Min(SelfEaseOutFrame, Duration - OtherEaseInFrame);
                }
            }
        }

        public bool Contains(float halfFrame)
        {
            return StartFrame < halfFrame && halfFrame < EndFrame;
        }

        //最后一个color标签的颜色
        public Color Color()
        {
            var colorAttribute = GetType().GetCustomAttributes<ColorAttribute>().ToArray();
            return colorAttribute[^1].Color / 255;
        }

        public string StartTimeText()
        {
            return $"StartTime: {StartTime.ToString("0.00")}S / StartFrame: {StartFrame}";
        }

        public string EndTimeText()
        {
            return $"EndTime: {EndTime.ToString("0.00")}S / StartFrame: {EndFrame}";
        }

        public string DurationText()
        {
            return $"Duration: {DurationTime.ToString("0.00")}S / {Duration}";
        }

        public virtual void RebindTimeline()
        {
            Track.RebindTimeline();
        }

        public virtual void RepaintInspector()
        {
            OnInspectorRepaint?.Invoke();
        }

        public virtual bool IsResizeable()
        {
            return (Capabilities & ClipCapabilities.Resizeable) == ClipCapabilities.Resizeable;
        }

        public virtual bool IsMixable()
        {
            return (Capabilities & ClipCapabilities.Mixable) == ClipCapabilities.Mixable;
        }

        public virtual bool IsClipInable()
        {
            return (Capabilities & ClipCapabilities.ClipInable) == ClipCapabilities.ClipInable;
        }
    }
    
    public abstract partial class SignalClip
    {
        protected SignalClip(Track track, int frame): base(track, frame)
        {
            EndFrame = StartFrame + 1;
        }
    }
#endif
}