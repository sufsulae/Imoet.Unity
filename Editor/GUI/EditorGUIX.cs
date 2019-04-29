// Imoet Unity Editor Tool
// Copyright Yusuf Sulaiman (C) 2017 <ucupxh@gmail.com>
using UnityEditor;
using UnityEngine;
using Imoet.Unity.Events;
using Sys = System;

namespace Imoet.UnityEditor
{
    public static class EditorGUIX
    {
        //private static bool m_exec;
        private static GUIStyle button;

        private static void CheckStyle()
        {
            if (button == null)
            {
                button = new GUIStyle(EditorStyles.miniButton);
                button.imagePosition = ImagePosition.ImageAbove;
            }
        }

        public static bool ToogleFullBar(Rect rect, bool value, GUIContent content)
        {
            CheckStyle();
            return GUI.Toggle(rect, value, content, button);
        }

        public static bool ToogleFullBar(Rect rect, bool value, string label)
        {
            CheckStyle();
            return GUI.Toggle(rect, value, new GUIContent(label), button);
        }

        public static bool ToogleWideBar(Rect rect, SerializedProperty property, string onDisableText = "Disabled", string onEnableText = "Enabled")
        {
            if (property.propertyType == SerializedPropertyType.Boolean)
            {
                if (property.hasMultipleDifferentValues)
                {
                    if (!EditorGUIX.ToogleWideBar(rect, true, property.displayName, "-", "-"))
                    {
                        property.boolValue = true;
                    }
                }
                else
                {
                    property.boolValue = EditorGUIX.ToogleWideBar(rect, property.boolValue, property.displayName);
                }
            }
            return property.boolValue;
        }

        public static bool ToogleWideBar(Rect rect, string label, SerializedProperty property, string onDisableText = "Disabled", string onEnableText = "Enabled")
        {
            if (property.propertyType == SerializedPropertyType.Boolean)
            {
                if (property.hasMultipleDifferentValues)
                {
                    if (!EditorGUIX.ToogleWideBar(rect, true, label, "-", "-"))
                    {
                        property.boolValue = true;
                    }
                }
                else
                {
                    property.boolValue = EditorGUIX.ToogleWideBar(rect, property.boolValue, label);
                }
            }
            return property.boolValue;
        }

        public static bool ToogleWideBar(Rect rect, bool value, string label = "", string onDisableText = "Disabled", string onEnableText = "Enabled")
        {
            return ToogleWideBar(rect, value, new GUIContent(label), onDisableText, onEnableText);
        }

        public static bool ToogleWideBar(Rect rect, bool value, GUIContent content, string onDisableText = "Disabled", string onEnableText = "Enabled")
        {
            CheckStyle();
            bool temp = value;
            string text = !temp ? onDisableText : onEnableText;
            EditorGUI.PrefixLabel(rect, content);
            rect.x += EditorGUIUtility.labelWidth;
            rect.width -= EditorGUIUtility.labelWidth;
            temp = GUI.Toggle(rect, temp, new GUIContent(text, content.tooltip), button);
            return temp;
        }

        public static void DisabledProperty(Rect rect, SerializedProperty property, bool disabled)
        {
            EditorGUI.BeginDisabledGroup(disabled);
            EditorGUI.PropertyField(rect, property);
            EditorGUI.EndDisabledGroup();
        }

        internal static void __drawUnityEventValue(Rect rect, SerializedProperty property, System.Type type) {
            var unityType = UnityEventExUtility.getUnityReadableType(type);
            if (property == null ||unityType <= (int)UnityEventExUtility.UnityReadableType.Void)
                return;
            
            SerializedProperty prop = null;
            switch (unityType) {
                case UnityEventExUtility.UnityReadableType.UnityObject:
                    prop = property.FindPropertyRelative("m_unityObject");
                    break;
                case UnityEventExUtility.UnityReadableType.Enum:
                    prop = property.FindPropertyRelative("m_enum");
                    break;
                default:
                    string tLower = "";
                    switch (type.Name)
                    {
                        case "Single":
                            tLower = "float";
                            break;

                        case "Int32":
                            tLower = "int";
                            break;

                        case "Boolean":
                            tLower = "bool";
                            break;

                        case "String":
                            tLower = "string";
                            break;

                        default:
                            tLower = type.Name;
                            break;
                    }
                    tLower = tLower.ToLower();
                    prop = property.FindPropertyRelative("m_" + tLower);
                    break;
            }
            var v = new Vector4();
            switch (unityType) {
                case UnityEventExUtility.UnityReadableType.Boolean:
                    bool b = prop.boolValue;
                    EditorGUI.BeginChangeCheck();
                    b = EditorGUI.Toggle(rect, b, EditorStyles.miniButton);
                    EditorGUI.LabelField(rect, b ? "True" : "False", UnityEditorSkin.centeredLabel);
                    if (EditorGUI.EndChangeCheck())
                        prop.boolValue = b;
                    break;
                case UnityEventExUtility.UnityReadableType.Quaternion:
                    Quaternion q = prop.quaternionValue;
                    v = new Vector4(q.x, q.y, q.z, q.w);
                    EditorGUI.BeginChangeCheck();
                    v = EditorGUI.Vector4Field(rect, "", v);
                    if (EditorGUI.EndChangeCheck())
                    {
                        q.Set(v.x, v.y, v.z, v.w);
                        prop.quaternionValue = q;
                    }
                    break;
                case UnityEventExUtility.UnityReadableType.Rect:
                    Rect r = prop.rectValue;
                    v = new Vector4(r.x, r.y, r.width, r.height);
                    EditorGUI.BeginChangeCheck();
                    v = EditorGUI.Vector4Field(rect, "", v);
                    if (EditorGUI.EndChangeCheck())
                    {
                        r.Set(v.x, v.y, v.z, v.w);
                        prop.rectValue = r;
                    }
                    break;
                case UnityEventExUtility.UnityReadableType.Enum:
                    string[] EnumNameList = Sys.Enum.GetNames(type);
                    int selectedValue = prop.intValue;
                    if (selectedValue >= EnumNameList.Length)
                        selectedValue = 0;
                    EditorGUI.BeginChangeCheck();
                    selectedValue = EditorGUI.Popup(rect, selectedValue, EnumNameList);
                    if (EditorGUI.EndChangeCheck())
                        prop.intValue = selectedValue;
                    break;
                case UnityEventExUtility.UnityReadableType.UnityObject:
                    Object inspectedValueObject = prop.objectReferenceValue;
                    inspectedValueObject = EditorGUI.ObjectField(rect, inspectedValueObject, type, true);
                    prop.objectReferenceValue = inspectedValueObject;
                    break;
                default:
                    EditorGUI.PropertyField(rect, prop, new GUIContent(""));
                    break;
            }
        }
    }
}