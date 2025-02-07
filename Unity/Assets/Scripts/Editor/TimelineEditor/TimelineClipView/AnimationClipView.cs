using System;
using UnityEditor;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class AnimationClipView: TimelineClipView
    {
        private UnityEngine.AnimationClip AnimationClip => (BBClip as BBAnimationClip).animationClip;
        

        protected override void MenuBuilder(DropdownMenu menu)
        {
            base.MenuBuilder(menu);
            menu.AppendAction("Open AnimationClip", _ =>
            {
                AnimationWindow animationWindow = EditorWindow.GetWindow<AnimationWindow>();
                animationWindow.animationClip = AnimationClip;
                animationWindow.Show();
            });
        }

        public override void InspectorAwake()
        {
            inspectorData = Activator.CreateInstance(typeof (AnimationClipInspectorData), (BBAnimationClip)BBClip) as AnimationClipInspectorData;
            inspectorData.InspectorAwake(FieldView);
        }

        public override void InsepctorUpdate()
        {
            inspectorData.InspectorUpdate(FieldView);
        }

        public override void InspectorDestroy()
        {
            inspectorData.InspectorDestroy(FieldView);
            FieldView.ClipInspector.Clear();
        }
    }
}