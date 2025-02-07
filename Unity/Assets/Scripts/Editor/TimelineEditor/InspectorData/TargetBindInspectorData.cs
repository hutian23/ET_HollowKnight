using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline.Editor
{
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

        [Button("保存", DirtyOnClick = false)]
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
}