namespace Timeline.Editor
{
    public class TargetBindMarkerView : MarkerView
    {
        private TargetBindKeyFrame KeyFrame => keyframeBase as TargetBindKeyFrame;
        private TargetBindTrackView BindTrackView => trackView as TargetBindTrackView;
        private BBTargetBindTrack BindTrack => BindTrackView.Track as BBTargetBindTrack;
        
        public override void Select()
        {
            base.Select();
            BBTargetBindInspectorData inspectorData = new(KeyFrame, BindTrack);
            inspectorData.InspectorAwake(FieldView);
            TimelineInspectorData.CreateView(FieldView.ClipInspector, inspectorData);
        }
    }
}