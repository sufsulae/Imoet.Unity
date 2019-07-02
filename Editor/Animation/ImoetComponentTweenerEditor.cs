using Imoet.Unity.Animation;
using UnityEditor;
namespace Imoet.UnityEditor
{
    [CustomEditor(typeof(ImoetComponentTweener))]
    public class ImoetComponentTweenerEditor : Editor
    {
        private SerializedProperty m_items;
        //private ReorderableList m_list;
        //private DynamicList2 m_list;
        private DynamicList4 m_list;
        private void OnEnable()
        {
            m_items = serializedObject.FindProperty("m_items");
            //m_list = new ReorderableList(serializedObject, m_items,false,true,true,true);
            //m_list = new DynamicList3(m_items, true);
        }
        public override void OnInspectorGUI()
        {
            if (m_list == null)
                m_list = new DynamicList4(m_items);
            m_list.Draw();
            serializedObject.ApplyModifiedProperties();
        }

        class Item {
            public SerializedProperty
            //Serialized Field
            m_setting,
            m_methodName,
            m_component,
            m_valueType,
            m_val;
            public Item(SerializedProperty prop) {
                m_setting = prop.FindPropertyRelative("m_settig");
                m_methodName = prop.FindPropertyRelative("m_methodName");
                m_component = prop.FindPropertyRelative("m_component");
                m_valueType = prop.FindPropertyRelative("m_valueType");
            }
        }
    }
}
