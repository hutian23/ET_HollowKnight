﻿using System;
using Sirenix.OdinInspector;
using UnityEngine.UIElements;

namespace Timeline.Editor
{
    public class TimelineInspectorData: SerializedScriptableObject
    {
        [HideLabel]
        [HideReferenceObjectPicker]
        public Object obj;

        public static TimelineInspectorData CreateView(VisualElement parent, Object _obj)
        {
            if (_obj == null) return null;
            TimelineInspectorData inspectorData = CreateInstance<TimelineInspectorData>();
            inspectorData.obj = _obj;

            var editor = UnityEditor.Editor.CreateEditor(inspectorData);
            IMGUIContainer container = new(() => { editor.OnInspectorGUI(); });
            parent.Clear();
            parent.Add(container);

            return inspectorData;
        }
    }
    
    public abstract class ShowInspectorData
    {
        protected ShowInspectorData(Object target)
        {
        }

        public abstract void InspectorAwake(TimelineFieldView fieldView);
        public abstract void InspectorUpdate(TimelineFieldView fieldView);
        public abstract void InspectorDestroy(TimelineFieldView fieldView);
    }
}