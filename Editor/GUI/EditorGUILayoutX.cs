// Imoet Unity Editor Tool
// Copyright Yusuf Sulaeman (C) 2020 <sufsulae@gmail.com>
using UnityEngine;
using UnityEditor;
namespace Imoet.UnityEditor
{
    public static class EditorGUILayoutX
    {
        private static GUIStyle m_button;
        private static GUIStyle _button
        {
            get
            {
                if (m_button == null)
                {
                    m_button = new GUIStyle(GUI.skin.button);
                    m_button.imagePosition = ImagePosition.ImageAbove;
                }
                return m_button;
            }
        }

        public static void BeginWideGUI()
        {
            BeginWideGUI(212f);
        }
        public static void BeginWideGUI(float offset)
        {
            if (!EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.wideMode = true;
                EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - offset;
            }
        }
        public static void EndWideGUI()
        {
            if (EditorGUIUtility.wideMode)
            {
                EditorGUIUtility.wideMode = false;
                EditorGUIUtility.labelWidth = 0f;
            }
        }

        public static void DisabledPropertyField(SerializedProperty property, bool disabled)
        {
            EditorGUI.BeginDisabledGroup(disabled);
            EditorGUILayout.PropertyField(property);
            EditorGUI.EndDisabledGroup();
        }

        #region ToggleFullBar
        public static bool ToggleFullBar(SerializedProperty property) {
            return ToggleFullBar(property, property.displayName);
        }
        public static bool ToggleFullBar(SerializedProperty property, GUIStyle style) {
            return ToggleFullBar(property, property.displayName, style);
        }
        public static bool ToggleFullBar(SerializedProperty property, string displayName) {
            return ToggleFullBar(property, displayName, _button);
        }
        public static bool ToggleFullBar(SerializedProperty property, string displayName, GUIStyle style) {
            if (property == null)
                return false;
            if (property.propertyType == SerializedPropertyType.Boolean) {
                property.boolValue = property.hasMultipleDifferentValues ? GUILayout.Toggle(false, "-", style) : GUILayout.Toggle(property.boolValue, displayName, style);
            }
            return property.boolValue;
        }
        public static bool ToggleFullBar(SerializedProperty property, string trueName, string falseName) {
            var name = property.boolValue ? trueName : falseName;
            return ToggleFullBar(property, name);
        }
        public static bool ToggleFullBar(SerializedProperty property, string trueName, string falseName, GUIStyle style) {
            var name = property.boolValue ? trueName : falseName;
            return ToggleFullBar(property, name, style);
        }

        public static bool ToggleFullBar(bool value,GUIContent content)
		{
			return GUILayout.Toggle(value,content,_button);
		}
        public static bool ToggleFullBar(bool value, GUIContent content, GUIStyle style) {
            return GUILayout.Toggle(value, content, style);
        }
		public static bool ToggleFullBar(bool value,string label)
		{
			return GUILayout.Toggle(value,label,_button);
		}
		public static bool ToggleFullBar(bool value, string label, GUIStyle style){
			return GUILayout.Toggle(value,label,style);
		}
        #endregion

        #region ToggleWideBar
        public static bool ToogleWideBar(bool value,string label = "",string onDisableText = "Disabled",string onEnableText = "Enabled")
		{
			return EditorGUIX.ToogleWideBar(GUILayoutUtility.GetRect(0,EditorGUIUtility.singleLineHeight),value,label,onDisableText, onEnableText);
		}
		public static bool ToogleWideBar(bool value,GUIContent content,string onDisableText = "Disabled",string onEnableText = "Enabled")
		{
			return EditorGUIX.ToogleWideBar(GUILayoutUtility.GetRect(0,EditorGUIUtility.singleLineHeight),value,content,onDisableText, onEnableText);
		}
		public static void ToogleWideBar(SerializedProperty property,string onDisableText = "Disabled",string onEnableText = "Enabled"){
			EditorGUIX.ToogleWideBar(GUILayoutUtility.GetRect(0,EditorGUIUtility.singleLineHeight),property,onDisableText,onEnableText);
		}
		public static void ToogleWideBar(SerializedProperty property,string label,string onDisableText,string onEnableText){
			EditorGUIX.ToogleWideBar(GUILayoutUtility.GetRect(0,EditorGUIUtility.singleLineHeight),label,property,onDisableText,onEnableText);
		}
        #endregion
    }
}
