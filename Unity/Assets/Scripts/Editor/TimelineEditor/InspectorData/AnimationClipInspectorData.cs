using System;
using ET;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Timeline.Editor
{
    [Serializable]
    public class AnimationClipInspectorData: ShowInspectorData
    {
        private BBAnimationClip Clip;
        private TimelineFieldView FieldView;

        [LabelText("ClipName: "), PropertyOrder(0)]
        public string ClipName;

        [Button("ReName"), PropertyOrder(1)]
        public void Rename()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() => { Clip.Name = ClipName; }, "Rename Clip");
        }

        [LabelText("Clip: "), PropertyOrder(3)]
        public AnimationClip AnimationClip;

        [LabelText("AnimationLength: "), PropertyOrder(4)]
        [ShowInInspector]
        public int animationLength => AnimationClip == null? 0 : (int)(AnimationClip.length * 60f);

        [Button("提取运动曲线", DirtyOnClick = false), PropertyOrder(6)]
        public void Rebind()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                Clip.animationClip = AnimationClip;
                Clip.rootMotionDict.Clear();

                foreach (EditorCurveBinding binding in AnimationUtility.GetCurveBindings(Clip.animationClip))
                {
                    AnimationCurve curve = AnimationUtility.GetEditorCurve(Clip.animationClip, binding);
                    string propertyName = binding.propertyName.Replace(".", "_");
                    AnimationCurve cloneCurve = MongoHelper.Clone(curve);
                    Clip.rootMotionDict.TryAdd(propertyName, cloneCurve);
                }
            }, "Extract AnimationCurve");
        }

        public AnimationClipInspectorData(object target): base(target)
        {
            Clip = target as BBAnimationClip;
            AnimationClip = Clip.animationClip;
            ClipName = Clip.Name;
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
}