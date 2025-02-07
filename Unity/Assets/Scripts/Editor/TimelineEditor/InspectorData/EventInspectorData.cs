using Sirenix.OdinInspector;

namespace Timeline.Editor
{
    public class EventInspectorData: ShowInspectorData
    {
        private TimelineFieldView FieldView;

        [HideReferenceObjectPicker, HideLabel]
        public EventInfo Info;

        [Button("保存")]
        public void Save()
        {
            FieldView.EditorWindow.ApplyModifyWithoutButtonUndo(() => { }, "Save info", false);
        }

        public EventInspectorData(object target): base(target)
        {
            Info = target as EventInfo;
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