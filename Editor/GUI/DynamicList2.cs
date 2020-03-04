using UnityEngine;
using System;
using UnityEditor;
using System.Collections.Generic;
using System.Collections;

namespace Imoet.UnityEditor {
    public interface IDynamicList2HeaderDrawer {
        float GetHeaderHeight(SerializedProperty propetrty);
        void DrawHeader(Rect rect, SerializedProperty property);
    }
    public interface IDynamicList2ItemBodyDrawer {
        float GetItemBodyHeight(SerializedProperty property);
        void DrawItemBody(Rect rect, SerializedProperty property);
    }
    public interface IDynamicList2ItemHeaderDrawer {
        float GetItemHeaderHeight(SerializedProperty property);
        void DrawItemHeader(Rect rect, SerializedProperty property);
    }

    public class DynamicList2 : IDynamicList2HeaderDrawer, IDynamicList2ItemBodyDrawer, IDynamicList2ItemHeaderDrawer
    {
        public IDynamicList2HeaderDrawer headerDrawer { get; set; }
        public IDynamicList2ItemHeaderDrawer itemHeaderDrawer { get; set; }
        public IDynamicList2ItemBodyDrawer itemBodyDrawer { get; set; }

        public SerializedProperty property { get { return m_property; } }
        public string propertyPath { get { return m_propertyPath; } }

        private string m_propertyPath;
        private SerializedProperty m_property;
        private SerializedObject m_propObject;
        private List<Element> m_arrayElement;

        private static Style m_style;

        public DynamicList2(SerializedProperty arrayProperty) : this(arrayProperty,true) { }
        public DynamicList2(SerializedProperty arrayProperty, bool reorderable) {
            if (arrayProperty.isArray)
            {
                m_property = arrayProperty;
                m_propertyPath = arrayProperty.propertyPath;
                m_propObject = arrayProperty.serializedObject;
                m_arrayElement = new List<Element>();
                headerDrawer = this;
                itemHeaderDrawer = this;
                itemBodyDrawer = this;
            } 
            else
                Debug.LogError("DynamicList only accept Array Property!");
        }

        public void Draw() {
            if (m_property == null)
                return;
            if (m_propObject.UpdateIfRequiredOrScript()){
                m_arrayElement = new List<Element>();
                for (int i = 0; i < m_property.arraySize; i++) {
                    var newArrayElement = new Element();
                    newArrayElement.property = m_property.GetArrayElementAtIndex(i);
                    newArrayElement.drawerHeader = itemHeaderDrawer;
                    newArrayElement.drawerBody = itemBodyDrawer;
                    m_arrayElement.Add(newArrayElement);
                }
            }
            var lastRect = default(Rect);
            foreach (var element in m_arrayElement) {
                lastRect = element.Draw(lastRect);
            }
            m_propObject.ApplyModifiedProperties();
        }

        public void RefreshView() {

        }

        #region Default Handler
        public float GetHeaderHeight(SerializedProperty property) {
            return 17f;
        }
        public void DrawHeader(Rect rect, SerializedProperty property) {

        }
        public float GetItemBodyHeight(SerializedProperty property) {
            return EditorGUI.GetPropertyHeight(property, true);
        }
        public void DrawItemBody(Rect rect, SerializedProperty property) {

        }
        public float GetItemHeaderHeight(SerializedProperty property) {
            return 17f;
        }
        public void DrawItemHeader(Rect rect, SerializedProperty property) {

        }
        #endregion

        private class Style
        {
            public GUIStyle normal = new GUIStyle();
            public GUIStyle normalTextMiddle = new GUIStyle();
            public GUIStyle box = new GUIStyle(GUI.skin.box);
            public GUIStyle header2 = new GUIStyle(EditorStyles.toolbar);
            public GUIContent plusButton = UnityEditorRes.IconToolbarPlus;
            public GUIContent minusButton = UnityEditorRes.IconToolbarMinus;
            public GUIStyle dragHandle = UnityEditorSkin.RLdraggingHandle;
            public GUIStyle header = UnityEditorSkin.RLheaderBackground;
            public GUIStyle headerLabel = UnityEditorSkin.midBoldLabel;
            public GUIStyle body = UnityEditorSkin.RLboxBackground;
            public GUIStyle itemBody = new GUIStyle(GUI.skin.box);
            public GUIStyle selectionBox = new GUIStyle(GUI.skin.button);

            public Style()
            {
                selectionBox.normal.background = selectionBox.focused.background;
                headerLabel.alignment = TextAnchor.MiddleLeft;
                normalTextMiddle.alignment = TextAnchor.MiddleCenter;
            }
        }

        class Element {
            public SerializedProperty property;
            public IDynamicList2ItemHeaderDrawer drawerHeader;
            public IDynamicList2ItemBodyDrawer drawerBody;

            public Rect Draw(Rect rect) {
                var lastRect = default(Rect);
                return lastRect;
            }
        }
    }
}