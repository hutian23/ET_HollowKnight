using System;
using ET;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline.Editor
{
    [Serializable]
    public class HitboxMarkerInspectorData: ShowInspectorData
    {
        [LabelText("当前帧: "), ReadOnly]
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

        [Button("刷新", DirtyOnClick = false)]
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
}