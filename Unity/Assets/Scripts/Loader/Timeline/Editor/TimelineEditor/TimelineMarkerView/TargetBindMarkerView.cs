namespace Timeline.Editor
{
    public class TargetBindMarkerView : MarkerView
    {
        private TargetBindInfo info => keyframeBase as TargetBindInfo;

        public override void Select()
        {
            base.Select();
            BBTargetBindInspectorData inspectorData = new(info);
            inspectorData.InspectorAwake(FieldView);
            TimelineInspectorData.CreateView(FieldView.ClipInspector, inspectorData);
        }
    }
}