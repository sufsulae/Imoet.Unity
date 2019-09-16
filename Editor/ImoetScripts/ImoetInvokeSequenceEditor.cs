using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Imoet.UnityEditor {
    [CustomEditor(typeof(Unity.Events.ImoetInvokeSequence))]
    public class ImoetInvokeSequenceEditor : Editor
    {
        private SerializedProperty
            m_autoExec,
            m_mode,
            m_items;
        private DynamicList m_list;

        private void OnEnable() {
            m_autoExec = serializedObject.FindProperty("m_autoExec");
            m_mode = serializedObject.FindProperty("m_mode");
            m_items = serializedObject.FindProperty("m_sequence");
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_autoExec);
            EditorGUILayout.PropertyField(m_mode);
            if (m_list == null)
                m_list = new DynamicList(m_items,true);
            m_list.Draw();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

