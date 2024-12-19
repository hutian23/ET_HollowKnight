using System;
using System.Collections.Generic;
using ET;
using UnityEngine;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineTrackView: VisualElement, ISelectable
    {
        public new class UxmlFactory: UxmlFactory<TimelineTrackView, UxmlTraits>
        {
        }

        public TimelineEditorWindow EditorWindow => FieldView.EditorWindow;
        protected readonly DropdownMenuHandler m_MenuHandler;
        private readonly DoubleMap<BBClip, TimelineClipView> ClipViewMap = new();
        public readonly List<MarkerView> markerViews = new();
        
        // public RuntimeTrack RuntimeTrack;
        // private BBTrack BBTrack => RuntimeTrack.Track;
        public BBTrack Track;
        
        public TimelineTrackView()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>($"VisualTree/TimelineTrackView");
            visualTree.CloneTree(this);
            AddToClassList("timelineTrack");

            RegisterCallback<PointerDownEvent>(OnPointerDown);
            RegisterCallback<PointerMoveEvent>(OnPointerMove);
            RegisterCallback<PointerOutEvent>(OnPointerOut);

            m_MenuHandler = new DropdownMenuHandler(MenuBuilder);
        }

        public virtual void Init(BBTrack track)
        {
            // RuntimeTrack = track;
            // int index = EditorWindow.RuntimePlayable.RuntimeTracks.IndexOf(track);
            Track = track;
            int index = EditorWindow.BBTimeline.Tracks.IndexOf(track);
            
            transform.position = new Vector3(0, index * 40, 0);

            //Init ClipView
            ClipViewMap.Clear();
            foreach (BBClip clip in Track.Clips)
            {
                TimelineClipView clipView = Activator.CreateInstance(Track.ClipViewType) as TimelineClipView;
                clipView.SelectionContainer = FieldView;
                clipView.Init(clip, this);
                
                Add(clipView);
                ClipViewMap.Add(clip, clipView);
                FieldView.SelectionElements.Add(clipView);
            }
        }

        public virtual void Refresh()
        {
            foreach (TimelineClipView clipViewValue in ClipViewMap.Values)
            {
                clipViewValue.Refresh();
            }
        }

        #region Selectable

        private bool m_Selected;
        public ISelection SelectionContainer { get; set; }
        public TimelineFieldView FieldView => SelectionContainer as TimelineFieldView;

        public override bool Overlaps(Rect rectangle)
        {
            return false;
        }

        public bool IsSelectable()
        {
            return false;
        }

        public bool IsSelected()
        {
            return m_Selected;
        }

        public void Select()
        {
            m_Selected = true;
            AddToClassList("selected");
            BringToFront();
        }

        public void UnSelect()
        {
            m_Selected = false;
            RemoveFromClassList("selected");
        }

        #endregion

        protected virtual void MenuBuilder(DropdownMenu menu)
        {
            menu.AppendAction("Add Clip",
                _ => { EditorWindow.ApplyModify(() => { Track.AddClip(FieldView.GetCurrentTimeLocator()); }, "Add Clip"); });
        }

        protected virtual void OnPointerDown(PointerDownEvent evt)
        {
            //当前选中了Clip
            foreach (TimelineClipView v in ClipViewMap.Values)
            {
                if (!v.InMiddle(evt.position)) continue;

                v.OnPointerDown(evt);
                evt.StopImmediatePropagation();
                return;
            }

            if (evt.button == 1)
            {
                m_MenuHandler.ShowMenu(evt);
                evt.StopImmediatePropagation();
            }
        }

        protected virtual void OnPointerMove(PointerMoveEvent evt)
        {
            foreach (TimelineClipView clipViewValue in ClipViewMap.Values)
            {
                clipViewValue.OnHover(false);
                if (clipViewValue.InMiddle(evt.position))
                {
                    clipViewValue.OnHover(true);
                    evt.StopImmediatePropagation();
                }
            }
        }

        protected virtual void OnPointerOut(PointerOutEvent evt)
        {
            foreach (TimelineClipView clipViewValue in ClipViewMap.Values)
            {
                clipViewValue.OnHover(false);
            }
        }
    }
}