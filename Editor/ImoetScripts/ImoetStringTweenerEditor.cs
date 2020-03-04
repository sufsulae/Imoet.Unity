using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Imoet.Unity.Animation;

namespace Imoet.UnityEditor {
    [CustomEditor(typeof(ImoetStringTweener))]
    public class ImoetStringTweenerEditor : Editor {
        SerializedProperty
            m_targetText,
            m_from, m_to,
            m_tweenSetting,
            m_tweenStringMode,
            m_tweenStringStyle;
        static Style m_style;
        private void OnEnable()
        {
            m_targetText = serializedObject.FindProperty("m_targetText");
            m_from = serializedObject.FindProperty("m_from");
            m_to = serializedObject.FindProperty("m_to");
            m_tweenSetting = serializedObject.FindProperty("m_tweenSetting");
            m_tweenStringMode = serializedObject.FindProperty("m_tweenStringMode");
            m_tweenStringStyle = serializedObject.FindProperty("m_tweenStringStyle");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            if (m_style == null)
                m_style = new Style();

            EditorGUILayout.BeginVertical(m_style.groupBackground);
            EditorGUILayout.LabelField("Target Field",m_style.boldLabel);
            EditorGUILayout.PropertyField(m_targetText,GUIContent.none);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(m_style.groupBackground);
            EditorGUILayout.LabelField("Target String", m_style.boldLabel);
            var l = GUILayoutUtility.GetLastRect();

            EditorGUILayout.BeginHorizontal(m_style.groupBackground);
            GUI.Box(new Rect(l.x+4,l.y+22,l.width,l.height),"Textnya");
            EditorGUILayout.PropertyField(m_from, GUIContent.none);
            EditorGUILayout.PropertyField(m_to, GUIContent.none);
            EditorGUILayout.EndHorizontal();

            GUILayout.Button("Preset Value");
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(m_style.groupBackground);
            EditorGUILayout.LabelField("Configuration", m_style.boldLabel);
            EditorGUILayout.PropertyField(m_tweenStringMode);
            EditorGUILayout.PropertyField(m_tweenStringStyle);
            EditorGUILayout.PropertyField(m_tweenSetting);
            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }

        class Style {
            public GUIStyle boldLabel = new GUIStyle(EditorStyles.boldLabel);
            public GUIStyle groupBackground = new GUIStyle(EditorStyles.helpBox);
        }
    }
}

