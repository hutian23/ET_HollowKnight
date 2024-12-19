using System.Linq;
using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class SubTimelineTrackView : TimelineTrackView
    {
        private SubTimelineTrack subTimelineTrack => Track as SubTimelineTrack;

        public override void Init(BBTrack track)
        {
            Track = track;

            int index = EditorWindow.BBTimeline.Tracks.IndexOf(track);
            transform.position = new Vector3(0, index * 40, 0);

            foreach (SubTimelineKeyFrame keyFrame in subTimelineTrack.KeyFrames)
            {
                SubTimelineMarkerView markerView = new();
                markerView.Init(this, keyFrame);

                markerViews.Add(markerView);
                Add(markerView);
            }
        }

        public override void Refresh()
        {
            foreach (MarkerView markerView in markerViews)
            {
                markerView.Refresh();
            }
        }

        private Vector3 localMousePos;

        protected override void OnPointerDown(PointerDownEvent evt)
        {
            int targetFrame = FieldView.GetClosestFrame(evt.localPosition.x);

            localMousePos = evt.localPosition;

            foreach (MarkerView markerView in markerViews.Where(markerView => markerView.InMiddle(targetFrame)))
            {
                markerView.OnPointerDown(evt);
            }

            if (evt.button == 1)
            {
                this.m_MenuHandler.ShowMenu(evt);
                evt.StopImmediatePropagation();
            }
        }

        #region Menu

        protected override void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Create KeyFrame", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                EditorWindow.ApplyModify(() =>
                {
                    subTimelineTrack.KeyFrames.Add(new SubTimelineKeyFrame(){frame = targetFrame});
                },"Create targetBind KeyFrame");
            },ContainKeyFrame(localMousePos.x)? DropdownMenuAction.Status.Hidden : DropdownMenuAction.Status.Normal);
            menu.AppendAction("Remove KeyFrame", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                SubTimelineKeyFrame keyFrame = subTimelineTrack.GetKeyFrame(targetFrame);
                EditorWindow.ApplyModify(() =>
                {
                    subTimelineTrack.KeyFrames.Remove(keyFrame);
                }, "Remove TargetBind KeyFrame");
            }, ContainKeyFrame(localMousePos.x)? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Copy KeyFrame", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                SubTimelineKeyFrame copyKeyFrame = MongoHelper.Clone(subTimelineTrack.GetKeyFrame(targetFrame));
                BBTimelineSettings.GetSettings().CopyTarget = copyKeyFrame;
            },ContainKeyFrame(localMousePos.x)? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Paste KeyFrame", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                SubTimelineKeyFrame keyFrame = BBTimelineSettings.GetSettings().CopyTarget as SubTimelineKeyFrame;
                if (keyFrame == null)
                {
                    return;
                }

                if (ContainKeyFrame(localMousePos.x))
                {
                    Debug.LogError($"already contain keyframe in: {targetFrame}");
                    return;
                }

                SubTimelineKeyFrame cloneKeyFrame = MongoHelper.Clone(keyFrame);
                cloneKeyFrame.frame = targetFrame;
                EditorWindow.ApplyModify(() =>
                {
                    subTimelineTrack.KeyFrames.Add(cloneKeyFrame);
                },"Paste TargetBind KeyFrame");
            },CanPaste()? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
        }
        
        private bool CanPaste()
        {
            SubTimelineKeyFrame keyFrame = BBTimelineSettings.GetSettings().CopyTarget as SubTimelineKeyFrame;
            return keyFrame != null;
        }

        private bool ContainKeyFrame(float x)
        {
            int frame = FieldView.GetClosestFrame(x);
            return subTimelineTrack.GetKeyFrame(frame) != null;
        }
        #endregion
    }
}