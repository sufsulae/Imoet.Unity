using Imoet.Unity.Animation;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEditor;
using Imoet.Unity.Utility;

namespace Imoet.UnityEditor
{
//    [CustomEditor(typeof(ImoetRectTransformTweener))]
//    public class ImoetRectTransformTweenerEditor : Editor {
//        private List<Item> m_items;
//        private SerializedProperty
//            m_tweenRectEnabled,
//            m_tweenRotEnabled,
//            m_tweenSclEnabled;
//        private SerializedProperty
//            m_tweenRectSetting,
//            m_tweenRotSetting,
//            m_tweenSclSetting;
//        private SerializedProperty m_tgtRectTransformCtrl;
//        private static Style m_style;
//        private ImoetRectTransformTweener m_tgt;

//        private void OnEnable() {
//            m_tgt = (ImoetRectTransformTweener)target;
//#if UNITY_2017_1_OR_NEWER
//            var prefabType = PrefabUtility.GetPrefabAssetType(m_tgt);
//            if (prefabType != PrefabAssetType.NotAPrefab && prefabType != PrefabAssetType.MissingAsset) {
//                m_tgt = null;
//            }
//#else
//            var prefabType = PrefabUtility.GetPrefabType(m_tgt);
//            if (prefabType != PrefabType.None && m_tgt.gameObject.scene.name == null) {
//                m_tgt = null;
//            }
//#endif
//            m_items = new List<Item>();
//            m_items.Add(new Item(serializedObject, "Rect") { parent = this });
//            m_items.Add(new Item(serializedObject, "Rot") { parent = this });
//            m_items.Add(new Item(serializedObject, "Scl") { parent = this });

//            bool isEnabled = false;
//            foreach (var item in m_items) {
//                if (item.focused) {
//                    isEnabled = true;
//                    break;
//                }
//            }
//            if (!isEnabled)
//                m_items[0].focused = true;
//            m_tgtRectTransformCtrl = serializedObject.FindProperty("m_tgtRectTransformCtrl");
//        }

//        public override void OnInspectorGUI() {
//            if (m_tgt) {
//                if (m_tgt.controlledRect == null || m_tgt.controlledRect.controller != m_tgt || m_tgt.controlledRect.transform.parent != m_tgt.transform.parent)
//                {
//                    m_tgt.DestroyController();
//                    m_tgt.BuildNewController();
//                }
//            }
//            if (m_style == null)
//                m_style = new Style();

//            serializedObject.Update();

//            EditorGUILayout.BeginHorizontal();
//            var headerRect = GUILayoutUtility.GetRect(0, 17f);
//            var iLen = m_items.Count;
//            var guiColor = GUI.color;
//            for (int i = 0; i < iLen; i++) {
//                var item = m_items[i];
//                if (item.focused)
//                    GUI.color = guiColor;
//                else
//                    GUI.color = new Color32(100, 100, 100, 255);

//                var tRect = new Rect(headerRect.x + headerRect.width * i / iLen, headerRect.y, headerRect.width / iLen, headerRect.height);
//                var toggle = GUI.Toggle(tRect,item.focused, new GUIContent("R"), m_style.tab);
//                if (item.focused != toggle && toggle) {
//                    foreach (var it in m_items)
//                        it.focused = item == it;
//                }
//            }
//            GUI.color = guiColor;
//            EditorGUILayout.EndHorizontal();

//            foreach (var item in m_items) { item.Draw(); }
//            serializedObject.ApplyModifiedProperties();
//        }

//        private void OnSceneGUI() {
//            if (!m_tgt || !m_tgt.controlledRect)
//                return;
//            var tgtCorner = m_tgt.rectTransform.GetWorldCorner3D();
//            var controllerCorner = m_tgt.controlledRect.rectTransform.GetWorldCorner3D();

//            Item activeProp = null;
//            foreach (var item in m_items) {
//                if (item.focused && item.tweenEnabled.boolValue) {
//                    activeProp = item;
//                    break;
//                }
//            }

//            if (activeProp == null)
//                return;

//            Color handleTempColor = Handles.color;
//            //Draw targetCorner
//            Handles.color = Color.red;
//            Handles.DrawPolyLine(tgtCorner.TopLeft, tgtCorner.BottomLeft, tgtCorner.BottomRight, tgtCorner.TopRight, tgtCorner.TopLeft);

//            //Draw controllerCorner
//            Handles.color = Color.green;
//            Handles.DrawPolyLine(controllerCorner.TopLeft, controllerCorner.BottomLeft, controllerCorner.BottomRight, controllerCorner.TopRight, controllerCorner.TopLeft);

//            //Draw Bridge
//            Handles.color = Color.blue;
//            Handles.DrawDottedLine(m_tgt.rectTransform.position, m_tgt.controlledRect.rectTransform.position, 10);

//            Handles.color = handleTempColor;
//        }

//        class Item {
//            public SerializedProperty tweenEnabled;
//            public SerializedProperty tweenSetting;

//            public bool focused { get { return tweenEnabled.isExpanded; } set { tweenEnabled.isExpanded = value; } }

//            public ImoetRectTransformTweenerEditor parent;

//            private string m_tittle;

//            public Item(SerializedObject sObj,string key) {
//                switch (key) {
//                    case "Rect":
//                        m_tittle = "Tween RectTransform";
//                        break;
//                    case "Rot":
//                        m_tittle = "Tween Rotation";
//                        break;
//                    case "Scl":
//                        m_tittle = "Tween Scale";
//                        break;
//                }
//                tweenEnabled = sObj.FindProperty("m_tween" + key + "Enabled");
//                tweenSetting = sObj.FindProperty("m_tween" + key + "Setting");
//            }

//            public void Draw() {
//                if (focused) {
//                    EditorGUILayout.BeginVertical(m_style.backgroundItem);
//                    tweenEnabled.boolValue = EditorGUILayoutX.ToogleFullBar(tweenEnabled.boolValue, m_tittle + (tweenEnabled.boolValue ? " Enabled" : " Disabled"),m_style.button);

//                    EditorGUILayout.BeginVertical(m_style.box);
//                    EditorGUILayout.LabelField("Tween Settings", m_style.headerText);
//                    EditorGUILayoutX.ToogleWideBar(tweenSetting.FindPropertyRelative("forceTween"));
//                    EditorGUILayoutX.ToogleWideBar(tweenSetting.FindPropertyRelative("resetValue"), "Always Reset","Disabled","Enabled");

//                    EditorGUILayoutX.BeginWideGUI();
//                    EditorGUILayout.PropertyField(tweenSetting.FindPropertyRelative("type"));
//                    EditorGUILayout.PropertyField(tweenSetting.FindPropertyRelative("duration"));
//                    EditorGUILayout.PropertyField(tweenSetting.FindPropertyRelative("mode"));
//                    EditorGUILayout.PropertyField(tweenSetting.FindPropertyRelative("direction"));
//                    EditorGUILayout.Slider(tweenSetting.FindPropertyRelative("progress"), 0.0f, 1f);
//                    EditorGUILayoutX.EndWideGUI();

//                    EditorGUILayout.EndVertical();
//                    EditorGUILayout.EndVertical();
//                }
//            }
//        }

//        class Style {
//            public GUIStyle headerItem = new GUIStyle(UnityEditorSkin.RLheaderBackground);
//            public GUIStyle headerText = new GUIStyle(UnityEditorSkin.centeredLabel);
//            public GUIStyle backgroundItem = new GUIStyle(UnityEditorSkin.RLboxBackground);
//            public GUIStyle button = new GUIStyle(EditorStyles.miniButton);
//            public GUIStyle box = new GUIStyle(UnityEditorSkin.helpBox);
//            public GUIStyle tab = new GUIStyle(UnityEditorSkin.RLheaderBackground);

//            public Style() {
//                headerText.fontStyle = FontStyle.Bold;
//            }
//        }
//    }
}