using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Imoet.Unity.Animation;

namespace Imoet.UnityEditor {
    [CustomPropertyDrawer(typeof(TweenSetting))]
    public sealed class ImoetUnityTweenSettingDrawer : PropertyDrawer {
        private static Style _style;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int tmpIndent = EditorGUI.indentLevel;
            if (tmpIndent > 0)
                EditorGUI.indentLevel--;
            if (_style == null)
                _style = new Style();
            if (property.isExpanded)
            {
                GUI.Box(position, "", _style.body);
                EditorGUIX.ToogleWideBar(new Rect(position.x + 5, position.y + 17f * 0 + 20f, position.width, 16f), property.FindPropertyRelative("forceTween"));
                EditorGUIX.ToogleWideBar(new Rect(position.x + 5, position.y + 17f * 1 + 20f, position.width, 16f), "Always Reset", property.FindPropertyRelative("resetValue"));
                EditorGUILayoutX.BeginWideGUI();
                EditorGUI.PropertyField(new Rect(position.x + 5, position.y + 17f * 2 + 20f, position.width - 10f, 16f), property.FindPropertyRelative("duration"));
                EditorGUI.PropertyField(new Rect(position.x + 5, position.y + 17f * 3 + 20f, position.width - 10f, 16f), property.FindPropertyRelative("mode"));
                EditorGUI.PropertyField(new Rect(position.x + 5, position.y + 17f * 4 + 20f, position.width - 10f, 16f), property.FindPropertyRelative("type"));
                EditorGUI.PropertyField(new Rect(position.x + 5, position.y + 17f * 5 + 20f, position.width - 10f, 16f), property.FindPropertyRelative("direction"));
                EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
                EditorGUI.Slider(new Rect(position.x + 5, position.y + 17f * 6 + 20f, position.width - 10f, 16f), property.FindPropertyRelative("progress"), 0.0f, 1.0f);
                EditorGUI.EndDisabledGroup();
                EditorGUILayoutX.EndWideGUI();
            }
            if (GUI.Button(new Rect(position.x, position.y, position.width, 18f), "", _style.header))
            {
                property.isExpanded = !property.isExpanded;
            }
            EditorGUI.LabelField(new Rect(position.x + 5, position.y, position.width - 5, position.height), property.displayName, _style.boldLabel);
            EditorGUI.indentLevel = tmpIndent;
        }

        private class Style
        {
            public GUIStyle boldLabel = new GUIStyle(EditorStyles.boldLabel);
            public GUIStyle header = UnityEditorSkin.RLheaderBackground;
            public GUIStyle body = UnityEditorSkin.RLboxBackground;
            public Style()
            {
                boldLabel.alignment = TextAnchor.UpperCenter;
                header.alignment = TextAnchor.MiddleCenter;
            }
        }
    }
}