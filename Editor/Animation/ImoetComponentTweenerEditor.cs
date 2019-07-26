using Imoet.Unity.Animation;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;

namespace Imoet.UnityEditor
{
    [CustomEditor(typeof(ImoetComponentTweener))]
    public class ImoetComponentTweenerEditor : Editor, IDynamicListHeaderDrawer, IDynamicListItemBodyDrawer, IDynamicListItemHeaderDrawer
    {
        private SerializedProperty m_items;
        private DynamicList m_list;
        private static Style m_style;
        private List<Item> m_cacheItem;

        #region Unity Drawer
        private void OnEnable() {
            m_cacheItem = new List<Item>();
            m_items = serializedObject.FindProperty("m_items");
        }
        public override void OnInspectorGUI() {
            if (m_style == null)
                m_style = new Style();
            
            if (m_list == null) {
                m_list = new DynamicList(m_items);
                m_list.drawerHeader = this;
                m_list.drawerItemBody = this;
                m_list.drawerItemHeader = this;
                m_list.reorderable = false;
            }
            m_list.Draw();
            serializedObject.ApplyModifiedProperties();
            _cleanCache();
        }
        #endregion

        #region Drawer Interface
        public float GetHeaderHeight(SerializedProperty property) {
            return EditorGUI.GetPropertyHeight(property, new GUIContent(property.displayName));
        }

        public void DrawHeader(Rect rect, SerializedProperty property) {
            GUI.Label(rect, "Components", m_style.headerText);
        }

        public float GetItemBodyHeight(SerializedProperty property) {
            return m_list.GetItemBodyHeight(property);
        }

        public void DrawItemBody(Rect rect, SerializedProperty property) {
            var singleLine = EditorGUIUtility.singleLineHeight;
            //Draw MethodSelector
            var item = _findItem(property);
            if(item == null)
            {
                item = new Item(property);
                m_cacheItem.Add(item);
            }
        }

        public float GetItemHeaderHeight(SerializedProperty property) {
            return m_list.GetHeaderHeight(property);
        }

        public void DrawItemHeader(Rect rect, SerializedProperty property) {
            m_list.DrawItemHeader(rect, property);
        }
        #endregion

        private Item _findItem(SerializedProperty property) {
            foreach (var item in m_cacheItem) {
                if (item.m_prop == property)
                    return item;
            }
            return null;
        }

        private void _cleanCache() {
            for (int i = 0; i < m_cacheItem.Count; i++) {
                if (m_cacheItem[i].m_prop == null)
                    m_cacheItem.RemoveAt(i);
            }
        }
        //Helper Class
        class Item {
            public SerializedProperty
            //Serialized Field
            m_setting,
            m_methodName,
            m_component,
            m_valueType,
            m_val,
            m_prop;
            public UnityMethodSelector m_selector;
            public Item(SerializedProperty prop) {
                m_prop = prop;
                m_setting = prop.FindPropertyRelative("m_setting");
                m_methodName = prop.FindPropertyRelative("m_methodName");
                m_component = prop.FindPropertyRelative("m_component");
                m_valueType = prop.FindPropertyRelative("m_valueType");
            }
        }
        class Style {
            public GUIStyle headerText = UnityEditorSkin.midBoldLabel;
            public GUIStyle labelText = new GUIStyle();
            public Style() {
                labelText.alignment = TextAnchor.MiddleCenter;
            }
        }
    }
}
