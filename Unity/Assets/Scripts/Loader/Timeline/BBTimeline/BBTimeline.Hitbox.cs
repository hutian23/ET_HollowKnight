using System;
using System.Collections.Generic;
using ET;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using Timeline.Editor;
using UnityEngine;

namespace Timeline
{
    [BBTrack("Hitbox")]
#if UNITY_EDITOR
    [Color(165, 032, 025)]
    [IconGuid("1dc9e96059838334696fb81dfec22393")]
#endif
    public class BBHitboxTrack: BBTrack
    {
        [OdinSerialize, NonSerialized]
        public List<HitboxKeyframe> Keyframes = new();

        public override Type RuntimeTrackType => typeof (RuntimeHitboxTrack);

        public HitboxKeyframe GetKeyframe(int targetFrame)
        {
            foreach (HitboxKeyframe keyframe in Keyframes)
            {
                if (keyframe.frame == targetFrame)
                {
                    return keyframe;
                }
            }

            return null;
        }

        public HitboxKeyframe GetClosestKeyframe(int targetFrame)
        {
            int closestFrame = -1;
            foreach (HitboxKeyframe keyFrame in Keyframes)
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
            return closestFrame == -1 ? null : GetKeyframe(closestFrame);
        }
        
#if UNITY_EDITOR
        public override Type TrackViewType => typeof (HitboxTrackView);
        public override int GetMaxFrame()
        {
            var max = 1;
            foreach (var keyframe in Keyframes)
            {
                if (keyframe.frame >= max)
                {
                    max = keyframe.frame;
                }
            }

            return max;
        }
#endif
    }

    [Serializable]
    public class HitboxKeyframe: BBKeyframeBase
    {
        [HideReferenceObjectPicker]
        public List<BoxInfo> boxInfos = new();
    }

    public enum HitboxType
    {
        None,
        Hit,
        Hurt,
        Throw,
        Squash,
        Proximity,
        Other,
        Gizmos
    }

    [Serializable]
    public class BoxInfo
    {
        [LabelText("判定框名: ")]
        public string boxName;

        [LabelText("判定框类型: ")]
        public HitboxType hitboxType;

        [LabelText("偏移: ")]
        public Vector2 center;

        [LabelText("大小: ")]
        public Vector2 size = Vector2.one;
    }
    
    //有点套娃 --- > Fixture.UserData ---> FixtureData ---> UserData
    public struct FixtureData
    {
        public long InstanceId;
        public int TriggerEnterId;
        public int TriggerStayId;
        public int TriggerExitId;
        
        public long LayerMask;
        public bool IsTrigger;
        
        public object UserData; 
    }

    public static class LayerType
    {
        public const int None = 0;
        public const int Ground = 2 << 0;
        public const int Unit = 2 << 1;
    }

#if UNITY_EDITOR
    [Serializable]
    public class HitboxMarkerInspectorData: ShowInspectorData
    {
        [LabelText("当前帧: "),ReadOnly]
        public int currentFrame;
        
        [LabelText("判定框类型: ")]
        public HitboxType HitboxType;

        [LabelText("判定框名: ")]
        public string HitboxName;

        [HideReferenceObjectPicker]
        [HideLabel]
        [HideInInspector]
        public HitboxKeyframe Keyframe;

        private TimelineFieldView fieldView;

        [PropertySpace(5)]
        [Button("新建判定框", DirtyOnClick = false)]
        private void CreateHitbox()
        {
            if (HitboxType is HitboxType.None)
            {
                Debug.LogError($"Hitbox type should not be none!");
                return;
            }

            this.fieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                GameObject parent = fieldView.EditorWindow.TimelinePlayer.gameObject.GetComponent<ReferenceCollector>()
                        .Get<GameObject>(HitboxType.ToString());
                GameObject child = new(HitboxName);
                child.transform.SetParent(parent.transform);
                child.transform.localPosition = Vector2.zero;
                CastBox castBox = child.AddComponent<CastBox>();
                castBox.info = new BoxInfo() { hitboxType = HitboxType, boxName = HitboxName };
            }, "Create hitbox", false);
        }

        [Button("刷新",DirtyOnClick = false)]
        private void Refresh()
        {
            RuntimeHitboxTrack.GenerateHitbox(fieldView.EditorWindow.TimelinePlayer, Keyframe);
        }

        [Button("保存", DirtyOnClick = false)]
        private void Save()
        {
            fieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                TimelinePlayer timelinePlayer = fieldView.EditorWindow.TimelinePlayer;
                Keyframe.boxInfos.Clear();
                foreach (CastBox castBox in timelinePlayer.GetComponentsInChildren<CastBox>())
                {
                    Keyframe.boxInfos.Add(MongoHelper.Clone(castBox.info));
                }
            }, "Save hitbox", false);
        }

        public HitboxMarkerInspectorData(object target): base(target)
        {
            Keyframe = target as HitboxKeyframe;
            currentFrame = Keyframe.frame;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            fieldView = _fieldView;
        }

        public override void InspectorUpdate(TimelineFieldView _fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView _fieldView)
        {
        }
    }
#endif

    #region Runtime

    public struct UpdateHitboxCallback
    {
        public long instanceId;

        public HitboxKeyframe Keyframe;
    }

    public class RuntimeHitboxTrack: RuntimeTrack
    {
        private TimelinePlayer timelinePlayer => RuntimePlayable.TimelinePlayer;

        public RuntimeHitboxTrack(RuntimePlayable runtimePlayable, BBTrack track): base(runtimePlayable, track)
        {
        }

        public override void Bind()
        {
        }

        public override void UnBind()
        {
#if UNITY_EDITOR
            ClearHitbox(timelinePlayer);
#endif
        }

        public override void SetTime(int targetFrame)
        {
            BBHitboxTrack hitboxTrack = Track as BBHitboxTrack;
            HitboxKeyframe _keyFrame = timelinePlayer.HasBindUnit? hitboxTrack.GetKeyframe(targetFrame) : hitboxTrack.GetClosestKeyframe(targetFrame);
            if (_keyFrame == null) return;

            if (timelinePlayer.HasBindUnit)
            {
                EventSystem.Instance.Invoke(new UpdateHitboxCallback() { instanceId = timelinePlayer.instanceId, Keyframe = _keyFrame });
            }
#if UNITY_EDITOR
            GenerateHitbox(timelinePlayer, _keyFrame);
#endif
        }

#if UNITY_EDITOR
        private static void ClearHitbox(TimelinePlayer timelinePlayer)
        {
            //1. 销毁HitboxTrack运行时产生的组件
            var goSet = new HashSet<GameObject>();
            foreach (var component in timelinePlayer.GetComponentsInChildren<Component>())
            {
                if (component.GetComponent<CastBox>() != null)
                {
                    goSet.Add(component.gameObject);
                }
            }
            foreach (GameObject go in goSet)
            {
                UnityEngine.Object.DestroyImmediate(go);
            }
        }
        
        public static void GenerateHitbox(TimelinePlayer timelinePlayer, HitboxKeyframe keyframe)
        {
            //1. 销毁HitboxTrack运行时产生的组件
            ClearHitbox(timelinePlayer);
            
            //2. 根据keyframe生成新的CastBox
            foreach (BoxInfo boxInfo in keyframe.boxInfos)
            {
                if (boxInfo.hitboxType is HitboxType.None) continue;

                GameObject parent = timelinePlayer
                        .GetComponent<ReferenceCollector>()
                        .Get<GameObject>(boxInfo.hitboxType.ToString());

                GameObject child = new(boxInfo.boxName);
                child.transform.SetParent(parent.transform);
                child.transform.localPosition = Vector2.zero;
                //深拷贝
                CastBox castBox = child.AddComponent<CastBox>();
                castBox.info = MongoHelper.Clone(boxInfo);
            }
        }
    }
#endif

    #endregion
}