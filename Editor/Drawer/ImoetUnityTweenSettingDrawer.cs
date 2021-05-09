using System.Collections.Generic;
using Imoet.Unity.Animation;
using UnityEditor;
using UnityEngine;

namespace Imoet.UnityEditor
{
    [CustomPropertyDrawer(typeof(TweenSetting))]
    public class ImoetUnityTweenSettingDrawer : PropertyDrawer {
        private static Style style {
            get {
                if (m_style == null)
                    m_style = new Style();
                return m_style;
            }
        }
        private static Style m_style;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true) + 5f;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height -= 3f;
            if (property.isExpanded)
            {
                var props = new SerializedProperty[] {
                    property.FindPropertyRelative("duration"),
                    property.FindPropertyRelative("type"),
                    property.FindPropertyRelative("mode"),
                    property.FindPropertyRelative("direction"),
                    property.FindPropertyRelative("startProgress"),
                };
                var propsLen = props.Length;
                //Draw Background
                GUI.Box(position, label.text + " (Tween Settings)", style.backgroundExpanded);
                //Draw Invisible Button
                if(GUI.Button(new Rect(position.x,position.y, position.width, 18f), "", style.invisible))
                    property.isExpanded = !property.isExpanded;
                //Start Drawing
                position.x += 4f;
                position.y += 17f;
                position.height = 16f;
                position.width -= 8f;
                for (int i = 0; i < propsLen-1; i++) {
                    EditorGUI.PropertyField(position, props[i]);
                    position.y += 18f;
                }
                EditorGUI.Slider(position, props[propsLen - 1], 0.0f, 1.0f);

            }
            else
            {
                if (GUI.Button(position, label.text + " (Tween Settings)", style.backgroundNonExpanded))
                    property.isExpanded = !property.isExpanded;
            }
        }

        private class Style {
            public GUIStyle invisible = UnityEditorSkin.invisibleButton;
            public GUIStyle backgroundNonExpanded = UnityEditorSkin.helpBox;
            public GUIStyle backgroundExpanded = UnityEditorSkin.helpBox;

            public Style() {
                backgroundExpanded.alignment = TextAnchor.UpperCenter;
                backgroundExpanded.fontStyle = FontStyle.Bold;

                backgroundNonExpanded.alignment = TextAnchor.MiddleCenter;
                backgroundNonExpanded.fontStyle = FontStyle.Bold;

                invisible.active.background = null;
                invisible.onActive.background = null;
            }
        }
    }
}