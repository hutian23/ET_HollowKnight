using System.Linq;
using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TargetBindTrackView : TimelineTrackView
    {
        private BBTargetBindTrack TargetBindTrack => Track as BBTargetBindTrack;

        public override void Init(BBTrack track)
        {
            Track = track;

            int index = EditorWindow.BBTimeline.Tracks.IndexOf(track);
            transform.position = new Vector3(0, index * 40, 0);

            foreach (TargetBindInfo info in TargetBindTrack.TargetBindInfos)
            {
                TargetBindMarkerView markerView = new();
                markerView.Init(this, info);

                this.markerViews.Add(markerView);
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

        private Vector2 localMousePos;

        protected override void OnPointerDown(PointerDownEvent evt)
        {
            int targetFrame = FieldView.GetClosestFrame(evt.localPosition.x);

            localMousePos = evt.localPosition;

            foreach (MarkerView markerView in markerViews.Where(markerView => markerView.InMiddle(targetFrame)))
            {
                markerView.OnPointerDown(evt);
            }

            //右键
            if (evt.button == 1)
            {
                // Open menu builder
                m_MenuHandler.ShowMenu(evt);
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
                    TargetBindTrack.TargetBindInfos.Add(new TargetBindInfo(){frame = targetFrame});
                },"Create targetBind KeyFrame");
            },ContainKeyFrame(localMousePos.x)? DropdownMenuAction.Status.Hidden : DropdownMenuAction.Status.Normal);
            menu.AppendAction("Remove KeyFrame", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                TargetBindInfo info = TargetBindTrack.GetInfo(targetFrame);
                EditorWindow.ApplyModify(() =>
                {
                    TargetBindTrack.TargetBindInfos.Remove(info);
                }, "Remove TargetBind KeyFrame");
            }, ContainKeyFrame(localMousePos.x)? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Copy KeyFrame", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                TargetBindInfo copyInfo = MongoHelper.Clone(TargetBindTrack.GetInfo(targetFrame));
                BBTimelineSettings.GetSettings().CopyTarget = copyInfo;
            },ContainKeyFrame(localMousePos.x)? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
            menu.AppendAction("Paste KeyFrame", _ =>
            {
                int targetFrame = FieldView.GetClosestFrame(localMousePos.x);
                TargetBindInfo info = BBTimelineSettings.GetSettings().CopyTarget as TargetBindInfo;
                if (info == null)
                {
                    return;
                }

                if (ContainKeyFrame(localMousePos.x))
                {
                    Debug.LogError($"already contain keyframe in: {targetFrame}");
                    return;
                }

                TargetBindInfo cloneInfo = MongoHelper.Clone(info);
                cloneInfo.frame = targetFrame;
                EditorWindow.ApplyModify(() =>
                {
                    TargetBindTrack.TargetBindInfos.Add(cloneInfo);
                },"Paste TargetBind KeyFrame");
            },CanPaste()? DropdownMenuAction.Status.Normal : DropdownMenuAction.Status.Hidden);
        }

        private bool CanPaste()
        {
            TargetBindInfo info = BBTimelineSettings.GetSettings().CopyTarget as TargetBindInfo;
            return info != null;
        }

        private bool ContainKeyFrame(float x)
        {
            int frame = FieldView.GetClosestFrame(x);
            return TargetBindTrack.GetInfo(frame) != null;
        }
        #endregion
    }
}