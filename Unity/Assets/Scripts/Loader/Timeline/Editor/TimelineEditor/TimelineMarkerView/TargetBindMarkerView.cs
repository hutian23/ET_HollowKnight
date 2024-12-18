namespace Timeline.Editor
{
    public class TargetBindMarkerView : MarkerView
    {
        private TargetBindKeyFrame KeyFrame => keyframeBase as TargetBindKeyFrame;

        public override void Select()
        {
            base.Select();
            BBTargetBindInspectorData inspectorData = new(this.KeyFrame);
            inspectorData.InspectorAwake(FieldView);
            TimelineInspectorData.CreateView(FieldView.ClipInspector, inspectorData);
        }
    }
}