using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Imoet.UnityEditor
{
    public class DynamicList3
    {
        private ReorderableList m_list;
        private SerializedProperty m_sProp;
        private static Style m_style;

        public DynamicList3(SerializedProperty prop,bool draggable) {
            if (prop.isArray)
                m_sProp = prop;

            m_list = new ReorderableList(prop.serializedObject, prop);
            m_list.displayAdd = false;
            m_list.displayRemove = false;
            m_list.draggable = draggable;

            m_list.elementHeightCallback = list_onItemHeight;
            m_list.drawHeaderCallback = list_onHeader;
            m_list.drawElementCallback = list_onItem;
        }

        public void Draw() {
            if (m_style == null)
                m_style = new Style();
            m_list.DoLayoutList();
        }

        public void Draw(Rect r) {
            if (m_style == null)
                m_style = new Style();
            m_list.DoList(r);
        }

        float list_onItemHeight(int id) {
            var prop = m_sProp.GetArrayElementAtIndex(id);
            if (prop.isExpanded)
                return EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName), true);
            else
                return EditorGUI.GetPropertyHeight(prop, new GUIContent(prop.displayName));
        }

        void list_onHeader(Rect r) {
            var btnR = new Rect(r.x + r.width - 15, r.y, 15, 15);
            GUI.Label(new Rect(r.x, r.y, r.width - 15, r.height), m_sProp.displayName, m_style.headerLabel);
            if (GUI.Button(btnR, m_style.plusButton.image,m_style.normal))
                m_sProp.InsertArrayElementAtIndex(m_sProp.arraySize);
        }

        void list_onItem(Rect r, int index, bool isActive, bool isFocused) {
            var itemHeaderRect = new Rect(r.x, r.y, r.width, 17);
            var delBtn = new Rect(itemHeaderRect.x + itemHeaderRect.width - 17, itemHeaderRect.y, 17, 17);
            if (GUI.Button(delBtn, m_style.minButton.image, m_style.normal))
                m_sProp.DeleteArrayElementAtIndex(index);

            var prop = m_sProp.GetArrayElementAtIndex(index);
            var itemTittleRect = new Rect(itemHeaderRect.x, itemHeaderRect.y, itemHeaderRect.width-17, itemHeaderRect.height);
            var itemBodyRect = new Rect(r.x, r.y + 17, r.width, r.height - 17);
            //Tittle
            if (GUI.Button(itemTittleRect, prop.displayName,m_style.tittle)) {
                prop.isExpanded = !prop.isExpanded;
            }
            if (prop.isExpanded) {
                //Background
                GUI.Box(new Rect(itemBodyRect.x - 16, itemBodyRect.y, itemBodyRect.width + 17, itemBodyRect.height), "", m_style.background);
            }
        }


        class Style {
            public GUIStyle normal = new GUIStyle();
            public GUIStyle tittle = UnityEditorSkin.centeredLabel;
            public GUIStyle background = GUI.skin.textField;
            public GUIStyle headerLabel = UnityEditorSkin.midBoldLabel;
            public GUIContent plusButton = UnityEditorRes.IconToolbarPlus;
            public GUIContent minButton = UnityEditorRes.IconToolbarMinus;
        }
    }
}
