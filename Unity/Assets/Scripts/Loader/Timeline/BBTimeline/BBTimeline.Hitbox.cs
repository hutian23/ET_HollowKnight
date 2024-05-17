﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ET;
using ET.Client;
using Sirenix.OdinInspector;
using Timeline.Editor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Timeline
{
    [BBTrack("Hitbox")]
    [Color(165, 032, 025)]
    [IconGuid("1dc9e96059838334696fb81dfec22393")]
    public class BBHitboxTrack: BBTrack
    {
        public override Type RuntimeTrackType => typeof (RuntimeHitboxTrack);
#if UNITY_EDITOR
        protected override Type ClipType => typeof (BBHitboxClip);
        public override Type ClipViewType => typeof (HitboxClipView);
#endif
    }

    [Color(165, 032, 025)]
    public class BBHitboxClip: BBClip
    {
        public Dictionary<int, List<BoxInfo>> boxInfoDict = new();

        public BBHitboxClip(int frame): base(frame)
        {
        }

#if UNITY_EDITOR
        public override Type ShowInInpsectorType => typeof (BbHitboxInspectorDataData);
#endif
    }

#if UNITY_EDITOR
    public enum HitboxType
    {
        Hit,
        Hurt,
        CounterHurt,
        Throw,
        Squash,
        Proximity
    }

    [Serializable]
    public class BoxInfo
    {
        [LabelText("判定框名: ")]
        public string boxName;

        [LabelText("绑定对象: ")]
        public string bindParent;

        [LabelText("判定框类型: ")]
        public HitboxType hitboxType;

        [LabelText("偏移: ")]
        public Vector2 center;

        [LabelText("大小: ")]
        public Vector2 size;
    }

    [Serializable]
    public class HitboxInfoInspector
    {
        [HideInInspector]
        public TimelineFieldView FieldView;

        [HideInInspector]
        public BBHitboxClip Clip;

        private TimelinePlayer timelinePlayer => FieldView.EditorWindow.TimelinePlayer;

        [LabelText("当前帧: ")]
        [Sirenix.OdinInspector.ReadOnly]
        public int frame;

        #region Create

        [FoldoutGroup(groupName: "创建判定框")]
        [LabelText("判定框名: ")]
        public string boxName;

        [FoldoutGroup(groupName: "创建判定框")]
        [LabelText("判定框类型: ")]
        public HitboxType HitBoxType;

        [FoldoutGroup(groupName: "创建判定框")]
        [LabelText("绑定对象: ")]
        public GameObject bindParent;

        [FoldoutGroup(groupName: "创建判定框")]
        [Sirenix.OdinInspector.Button("创建")]
        public void CreateHitbox()
        {
            //refer
            ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
            if (refer == null)
            {
                refer = timelinePlayer.gameObject.AddComponent<ReferenceCollector>();
            }

            refer.Remove(bindParent.name);
            refer.Add(bindParent.name, bindParent);

            GameObject go = Object.Instantiate(BBTimelineSettings.GetSettings().hitboxPrefab, bindParent.transform);
            go.name = boxName;

            CastBox castBox = go.GetComponent<CastBox>();
            BoxInfo info = new()
            {
                boxName = boxName,
                hitboxType = HitBoxType,
                center = Vector2.zero,
                size = Vector3.one,
                bindParent = bindParent.name
            };
            castBox.info = info;
        }

        [FoldoutGroup(groupName: "创建判定框")]
        [Sirenix.OdinInspector.Button("保存当前判定框")]
        public void SaveHitbox()
        {
            if (Clip.boxInfoDict.ContainsKey(frame))
            {
                Clip.boxInfoDict.Remove(frame);
            }

            var castBoxes = timelinePlayer.GetComponentsInChildren<CastBox>();
            if (castBoxes.Length == 0) return;

            List<BoxInfo> boxInfos = new List<BoxInfo>();
            foreach (var castBox in castBoxes)
            {
                BoxInfo boxInfo = MongoHelper.Clone(castBox.info);
                boxInfos.Add(boxInfo);
            }

            Clip.boxInfoDict.TryAdd(frame, boxInfos);
        }

        [FoldoutGroup(groupName: "创建判定框")]
        [Sirenix.OdinInspector.Button("清空判定框")]
        public void ClearHitbox()
        {
            foreach (var castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                Object.DestroyImmediate(castBox.gameObject);
            }
        }

        [FoldoutGroup(groupName: "创建判定框")]
        [Sirenix.OdinInspector.Button("更新判定框")]
        public void UpdateHitbox()
        {
            ClearHitbox();
        }

        #endregion
    }

    [Serializable]
    public class BbHitboxInspectorDataData: ShowInspectorData
    {
        [Sirenix.OdinInspector.ReadOnly]
        [LabelText("当前帧: ")]
        public int ClipInFrame;

        // [LabelText("判定框信息: ")]
        // [HideReferenceObjectPicker]
        // [ListDrawerSettings(ShowFoldout = true, ShowIndexLabels = true, ListElementLabelName = "boxName", IsReadOnly = true)]
        // public List<BoxInfo> BoxInfos = new();

        #region Create

        private TimelineFieldView FieldView;
        private TimelinePlayer timelinePlayer => FieldView.EditorWindow.TimelinePlayer;

        [FoldoutGroup(groupName: "创建判定框")]
        [LabelText("判定框名: ")]
        public string boxName;

        [FoldoutGroup(groupName: "创建判定框")]
        [LabelText("判定框类型: ")]
        public HitboxType HitBoxType;

        [FoldoutGroup(groupName: "创建判定框")]
        [LabelText("绑定对象: ")]
        public GameObject bindParent;

        [FoldoutGroup(groupName: "创建判定框")]
        [Sirenix.OdinInspector.Button("创建")]
        public void CreateHitbox()
        {
            //refer 
            ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
            if (refer == null)
            {
                refer = timelinePlayer.gameObject.AddComponent<ReferenceCollector>();
            }

            refer.Remove(bindParent.name);
            refer.Add(bindParent.name, bindParent);

            //create hit prefab
            GameObject go = Object.Instantiate(BBTimelineSettings.GetSettings().hitboxPrefab, bindParent.transform);
            go.name = boxName;

            CastBox castBox = go.GetComponent<CastBox>();
            BoxInfo info = new()
            {
                boxName = boxName,
                hitboxType = HitBoxType,
                center = Vector2.zero,
                size = Vector3.one,
                bindParent = bindParent.name
            };
            castBox.info = info;
        }

        [Sirenix.OdinInspector.Button("清空")]
        public void ClearHitbox()
        {
            //Clear hitboxes
            foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                Object.DestroyImmediate(castBox.gameObject);
            }
        }

        [Sirenix.OdinInspector.Button("保存")]
        public void SaveHitbox()
        {
            List<BoxInfo> boxInfos = new List<BoxInfo>();
            foreach (var castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                BoxInfo info = MongoHelper.Clone(castBox.info);
                boxInfos.Add(info);
            }

            if (hitboxClip.boxInfoDict.TryGetValue(ClipInFrame, out List<BoxInfo> infos))
            {
                hitboxClip.boxInfoDict.Remove(ClipInFrame);
            }

            if (boxInfos.Count == 0) return;
            hitboxClip.boxInfoDict.TryAdd(ClipInFrame, boxInfos);
        }

        [Sirenix.OdinInspector.Button("复制判定框信息")]
        public void CopyCurrentHitbox()
        {
            List<BoxInfo> infos = new List<BoxInfo>();
            foreach (var castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                BoxInfo info = MongoHelper.Clone(castBox.info);
                infos.Add(info);
            }

            BBTimelineSettings.GetSettings().CopyTarget = infos;
        }

        [Sirenix.OdinInspector.Button("黏贴判定框信息")]
        public void PasteCurrentHitbox()
        {
            if (BBTimelineSettings.GetSettings().CopyTarget is not List<BoxInfo> infos) return;

            ClearHitbox();

            //Create hitbox
            ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
            foreach (var boxInfo in infos)
            {
                GameObject parent = refer.Get<GameObject>(boxInfo.bindParent);
                GameObject go = Object.Instantiate(BBTimelineSettings.GetSettings().hitboxPrefab, parent.transform);
                go.name = boxInfo.boxName;
                go.GetComponent<CastBox>().info = boxInfo;
            }
        }

        #endregion

        private BBHitboxClip hitboxClip;

        public BbHitboxInspectorDataData(object target): base(target)
        {
            hitboxClip = target as BBHitboxClip;
        }

        public override void InspectorAwake(TimelineFieldView fieldView)
        {
            FieldView = fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView fieldView)
        {
            ClipInFrame = fieldView.GetCurrentTimeLocator() - hitboxClip.StartFrame;
        }

        public override void InspectorDestroy(TimelineFieldView fieldView)
        {
        }
    }

#endif

    #region Runtime

    public class RuntimeHitboxTrack: RuntimeTrack
    {
        private int currentFrame;
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;

        public RuntimeHitboxTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
            //Clear hitboxes
            foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                Object.DestroyImmediate(castBox.gameObject);
            }
        }

        public override void SetTime(int targetFrame)
        {
            //帧更新
            if (currentFrame == targetFrame) return;
            currentFrame = targetFrame;

            foreach (var clip in Track.Clips)
            {
                if (clip.InMiddle(targetFrame))
                {
                    BBHitboxClip hitboxClip = clip as BBHitboxClip;
                    int clipFrame = targetFrame - clip.StartFrame;
                    if (!hitboxClip.boxInfoDict.TryGetValue(clipFrame, out List<BoxInfo> boxInfos)) return;

                    //Clear hitboxes
                    foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
                    {
                        Object.DestroyImmediate(castBox.gameObject);
                    }

                    //Create hitbox
                    ReferenceCollector refer = timelinePlayer.GetComponent<ReferenceCollector>();
                    foreach (var boxInfo in boxInfos)
                    {
                        GameObject parent = refer.Get<GameObject>(boxInfo.bindParent);
                        GameObject go = Object.Instantiate(BBTimelineSettings.GetSettings().hitboxPrefab, parent.transform);
                        go.name = boxInfo.boxName;
                        go.GetComponent<CastBox>().info = boxInfo;
                    }

                    return;
                }
            }

            //Clear hitboxes
            foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
            {
                Object.DestroyImmediate(castBox.gameObject);
            }
        }

        public override void RuntimMute(bool value)
        {
        }
    }

    #endregion
}