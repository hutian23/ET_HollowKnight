namespace Timeline.Editor
{
    public class SubTimelineMarkerView : MarkerView
    {
        private SubTimelineKeyFrame KeyFrame => keyframeBase as SubTimelineKeyFrame;
        private SubTimelineTrackView TrackView => trackView as SubTimelineTrackView;
        private SubTimelineTrack timelineTrack => TrackView.Track as SubTimelineTrack;

        public override void Select()
        {
            base.Select();
            SubTimelineInspectorData inspectorData = new(KeyFrame, timelineTrack);
            inspectorData.InspectorAwake(FieldView);
            TimelineInspectorData.CreateView(FieldView.ClipInspector, inspectorData);
        }
    }
}