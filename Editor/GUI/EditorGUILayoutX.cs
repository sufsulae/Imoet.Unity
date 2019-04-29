// Imoet Unity Editor Tool
// Copyright Yusuf Sulaiman (C) 2015 <yusufxh@ymail.com>
using UnityEngine;
using UnityEditor;
namespace Imoet.UnityEditor
{
	public static class EditorGUILayoutX
	{
		private static bool m_exec;
		private static GUIStyle button;
		private static void CheckStyle()
		{
			if (button == null)
			{
				button = new GUIStyle(GUI.skin.button);
				button.imagePosition = ImagePosition.ImageAbove;
			}
		}
		/// <summary>
		/// Make wider Editor GUI
		/// </summary>
		public static void BeginWideGUI()
		{
			BeginWideGUI(212f);
		}
		/// <summary>
		/// Make a wider Editor GUI with certain offset
		/// </summary>
		public static void BeginWideGUI(float offset)
		{
			if(!EditorGUIUtility.wideMode)
			{
				EditorGUIUtility.wideMode = true;
				EditorGUIUtility.labelWidth = EditorGUIUtility.currentViewWidth - offset;
				m_exec = true;
			}
		}
		public static void EndWideGUI()
		{
			if(m_exec)
			{
				EditorGUIUtility.wideMode = false;
				EditorGUIUtility.labelWidth = 0f;
				m_exec = false;
			}
		}

		public static void DisabledPropertyField(SerializedProperty property,bool disabled)
		{
			EditorGUI.BeginDisabledGroup(disabled);
			EditorGUILayout.PropertyField(property);
			EditorGUI.EndDisabledGroup();
		}
		public static bool ToogleFullBar(SerializedProperty property){
			if(property == null)
				return false;
			CheckStyle();
			if(property.propertyType == SerializedPropertyType.Boolean){
				if(property.hasMultipleDifferentValues){
					property.boolValue = GUILayout.Toggle(false,"-",button);
				}
				else{
					property.boolValue = GUILayout.Toggle(property.boolValue,property.displayName,button);
				}
			}
			return property.boolValue;
		}
		public static bool ToogleFullBar(bool value,GUIContent content)
		{
			CheckStyle ();
			return GUILayout.Toggle(value,content,button);
		}
		public static bool ToogleFullBar(bool value,string label)
		{
			CheckStyle ();
			return GUILayout.Toggle(value,label,button);
		}
		public static bool ToogleFullBar(bool value, string label, GUIStyle style){
			CheckStyle();
			return GUILayout.Toggle(value,label,style);
		}
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
	}
}
