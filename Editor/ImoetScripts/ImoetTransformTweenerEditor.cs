using System.Collections;
using System.Collections.Generic;
using Imoet.Unity;
using Imoet.Unity.Animation;
using UnityEditor;
using UnityEngine;

namespace Imoet.UnityEditor {
    [CustomEditor(typeof(ImoetTransformTweener))]
    public class ImoetTransformTweenerEditor : ImoetUnityEditor {

        private static List<bool> m_tabs;
        private static Skin m_skin;
        private static int m_selectedTabs;
        private int m_currentObjectSpace;
        private int m_changedObjectSpace;

        private SerializedProperty m_drawedProperty;

        public void OnEnable() {
            if (m_tabs == null) {
                m_tabs = new List<bool>(new bool[3]);
            }  
        }

        public override void OnInspectorGUI() {
            //Instance Skin
            if (m_skin == null)
                m_skin = new Skin();

            serializedObject.Update();

            //Draw Tabs
            var tabArea = GUILayoutUtility.GetRect(0, 17);
            for (int i = 0; i < m_tabs.Count; i++) {
                m_tabs[i] = i == m_selectedTabs;
                var tabHeaderPos = new Rect(tabArea.x + i * (tabArea.width / m_tabs.Count), tabArea.y, tabArea.width / m_tabs.Count, tabArea.height);
                GUI.color = i == m_selectedTabs ? m_skin.guiNormalColor : m_skin.tabUnselectedColor;
                var state = GUI.Toggle(tabHeaderPos, m_tabs[i], new GUIContent(""), m_skin.tab);
                if (state != m_tabs[i])
                    m_selectedTabs = i;
            }
            GUI.color = m_skin.guiNormalColor;

            //Draw Property
            m_drawedProperty = null;
            string displayName = null;
            switch (m_selectedTabs) {
                case 0:
                    m_drawedProperty = getProperty("m_pos");
                    displayName = "Tween Position";
                    break;
                case 1:
                    m_drawedProperty = getProperty("m_rot");
                    displayName = "Tween Rotation";
                    break;
                case 2:
                    m_drawedProperty = getProperty("m_scl");
                    displayName = "Tween Scale";
                    break;
            }
            _drawTabItem(m_drawedProperty, displayName);

            serializedObject.ApplyModifiedProperties();
        }

        public void OnSceneGUI() {
            if (!serializedObject.UpdateIfRequiredOrScript())
                return;
            var transform = ((ImoetTransformTweener)target).transform;
            SerializedProperty from = null;
            SerializedProperty to = null;
            if (m_drawedProperty != null) {
                switch (m_selectedTabs) {
                    case 0: //Position
                        from = getSubProperty(m_drawedProperty, "from");
                        to = getSubProperty(m_drawedProperty, "to");
                        break;
                    //TODO: For now i ignore this, next time i will make guider with Bound
                    //case 1: //Rotation
                    //    break;
                    //case 2: //Scale
                    //    break;
                }
            }
        }

        private void _drawTabItem(SerializedProperty prop, string displayName) {
            if (prop == null)
                return;
            prop.isExpanded = false;

            //Sub Property Variable
            var enable = getSubProperty(prop, "m_enable");
            var lockedTgt = getSubProperty(prop, "m_sameAsSource");
            SerializedProperty objectSpace = prop.name == "m_scl" ? null : getSubProperty(prop, "m_space");

            EditorGUILayout.BeginVertical(m_skin.tabItemBackground);
            EditorGUILayoutX.ToggleFullBar(enable, displayName + " Enabled", displayName + " Disabled", m_skin.toggle);
            EditorGUI.BeginDisabledGroup(!enable.boolValue);
            EditorGUILayoutX.BeginWideGUI();

            //Draw "From" Property
            EditorGUILayout.BeginHorizontal();
            EditorGUILayoutX.DisabledPropertyField(getSubProperty(prop, "from"), lockedTgt.enumValueIndex == 1);
            var lockFrom = EditorGUILayoutX.ToggleFullBar(lockedTgt.enumValueIndex == 1, "L", m_skin.lockButton);
            if (!lockFrom && lockedTgt.enumValueIndex == 1)
                lockedTgt.enumValueIndex = 0;
            else if (lockFrom)
                lockedTgt.enumValueIndex = 1;

            EditorGUILayout.EndHorizontal();
            
            //Draw "To" Property
            EditorGUILayout.BeginHorizontal();
            EditorGUILayoutX.DisabledPropertyField(getSubProperty(prop, "to"), lockedTgt.enumValueIndex == 2);
            var lockTo = EditorGUILayoutX.ToggleFullBar(lockedTgt.enumValueIndex == 2, "L", m_skin.lockButton);
            if (!lockTo && lockedTgt.enumValueIndex == 2)
                lockedTgt.enumValueIndex = 0;
            else if (lockTo)
                lockedTgt.enumValueIndex = 2;
            EditorGUILayout.EndHorizontal();

            //Draw "ObjectSpace" Property
            if (objectSpace != null) {
                var newObjSpace = (ObjectSpace)EditorGUILayout.EnumPopup((ObjectSpace)objectSpace.enumValueIndex);
                if (newObjSpace != (ObjectSpace)objectSpace.enumValueIndex) {
                    objectSpace.enumValueIndex = (int)newObjSpace;
                }
            }
                

            //Draw "Tween Setting" Property
            EditorGUILayout.PropertyField(getSubProperty(prop, "setting"),true);


            EditorGUILayoutX.EndWideGUI();
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.EndVertical();
        }

        private class Skin {
            public Color guiNormalColor;
            public Color tabUnselectedColor = new Color32(100, 100, 100, 255);
            public GUIStyle tab = new GUIStyle(UnityEditorSkin.RLheaderBackground);
            public GUIStyle tabItemBackground = new GUIStyle(UnityEditorSkin.RLboxBackground);
            public GUIStyle toggle = new GUIStyle(EditorStyles.miniButton);
            public GUIStyle lockButton = new GUIStyle(EditorStyles.miniButton);
            public Skin() {
                guiNormalColor = GUI.color;
                lockButton.fixedWidth = 20;
            }
        }
    }
}

