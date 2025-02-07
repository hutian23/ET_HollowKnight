using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Timeline.Editor
{
    [Serializable]
    public class SubTimelineInspectorData: ShowInspectorData
    {
        [LabelText("当前帧: "), ReadOnly, PropertyOrder(0)]
        public int curFrame;

        [LabelText("子对象: "), PropertyOrder(1)]
        public GameObject referGo;

        [LabelText("子行为: ")]
        public BBTimeline subTimeline;

        [LabelText("目标帧: "), PropertyOrder(3)]
        public int timelineFrame;

        [Button("保存"), PropertyOrder(4)]
        public void Save()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() =>
            {
                KeyFrame.TimelineFrame = this.timelineFrame;

                if (referGo == null)
                {
                    Track.referName = string.Empty;
                }
                // 因为GameObject不能跨场景保存，因此仅保存引用
                else
                {
                    ReferenceCollector refer = FieldView.EditorWindow.TimelinePlayer.GetComponent<ReferenceCollector>();
                    refer.Remove(referGo.name);
                    refer.Add(referGo.name, referGo);
                    Track.referName = referGo.name;
                }

                Track.subTimeline = subTimeline;
            }, "Save SubTimeline KeyFrame");
        }

        [Button("预览", DirtyOnClick = false), PropertyOrder(5)]
        public void PreView()
        {
            TimelinePlayer referPlayer = referGo.GetComponent<TimelinePlayer>();
            referPlayer.Init(subTimeline);
            referPlayer.Evaluate(timelineFrame);
        }

        private readonly SubTimelineKeyFrame KeyFrame;
        private readonly SubTimelineTrack Track;
        private TimelineFieldView FieldView;

        public SubTimelineInspectorData(object target, SubTimelineTrack track): base(target)
        {
            KeyFrame = target as SubTimelineKeyFrame;
            Track = track;
        }

        public override void InspectorAwake(TimelineFieldView _fieldView)
        {
            FieldView = _fieldView;
            curFrame = KeyFrame.frame;

            timelineFrame = KeyFrame.TimelineFrame;
            subTimeline = Track.subTimeline;

            //Find Refer GameObject
            ReferenceCollector refer = FieldView.EditorWindow.TimelinePlayer.GetComponent<ReferenceCollector>();
            referGo = refer.Get<GameObject>(Track.referName);
        }

        public override void InspectorUpdate(TimelineFieldView _fieldView)
        {
        }

        public override void InspectorDestroy(TimelineFieldView _fieldView)
        {
        }
    }
}