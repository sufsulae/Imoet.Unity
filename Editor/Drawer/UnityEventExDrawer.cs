using Imoet.Unity.Events;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Collections.Generic;
using System.Reflection;
using Imoet.Unity.Utility;

namespace Imoet.UnityEditor {
    [CustomPropertyDrawer(typeof(UnityEventEx), true)]
    public class UnityEventExDrawer : PropertyDrawer
    {
        private ReorderableList m_list;
        private GenericMenu m_menu;
        private Dictionary<string, ReorderableList> m_listContainer;

        private ReorderableList temp_inspectedList;
        private SerializedProperty temp_inspectedProp;
        private RectOffset temp_uiMargin;

        public UnityEventExDrawer() {
            m_listContainer = new Dictionary<string, ReorderableList>();
            temp_uiMargin = new RectOffset(1, 1, 1, 1);
        }

        //private const Type[] m_allowedTyped ;

        #region Unity Thread
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.hasMultipleDifferentValues) {
                temp_inspectedProp = property;
                temp_inspectedList = _getStoredList(property.FindPropertyRelative("m_methodList"));
                return temp_inspectedList.GetHeight();
            }
            return 34f;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!property.hasMultipleDifferentValues) {
                EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode);
                temp_inspectedList.DoList(position);
                EditorGUI.EndDisabledGroup();
            }
        }
        #endregion

        #region Function
        private ReorderableList _getStoredList(SerializedProperty prop)
        {
            ReorderableList res = null;
            if (!m_listContainer.TryGetValue(prop.propertyPath, out res))
            {
                res = new ReorderableList(prop.serializedObject, prop);
                res.elementHeight = 34f;
                res.drawHeaderCallback = _list_drawHeaderCallback;
                res.drawElementCallback = _list_drawElementCallback;
                m_listContainer.Add(prop.propertyPath, res);
            }
            return res;
        }
        #endregion

        #region ReorderableList Callback
        private void _list_drawHeaderCallback(Rect r)
        {
            //Draw Execution Mode Enum
            EditorGUI.LabelField(r, new GUIContent(temp_inspectedProp.displayName));
            SerializedProperty exeMode = temp_inspectedProp.FindPropertyRelative("m_exeMode");
            exeMode.enumValueIndex = EditorGUI.Popup(new Rect(r.x + (r.width - r.width / 3), r.y, r.width / 3 + 3, r.height - 2), exeMode.enumValueIndex, exeMode.enumDisplayNames, EditorStyles.toolbarPopup);
        }
        private void _list_drawElementCallback(Rect rect, int index, bool isActive, bool isFocused)
        {
            var prop = temp_inspectedList.serializedProperty.GetArrayElementAtIndex(index);
            var propItem = new PropItem(prop);
            //We split this element area into 3 region
            //Region 1 (Enable Button & Obj Reff)
            var lRect = new Rect(rect.x, rect.y, rect.width / 3.5f, rect.height);
            //Enable Button
            propItem.enable.boolValue = EditorGUIX.ToogleFullBar(temp_uiMargin.Remove(new Rect(lRect.x, lRect.y, lRect.width, lRect.height / 2f)), propItem.enable.boolValue, "Enable");
            EditorGUI.BeginDisabledGroup(!propItem.enable.boolValue);
            //Object Refference
            EditorGUI.ObjectField(temp_uiMargin.Remove(new Rect(lRect.x, lRect.y + lRect.height / 2f, lRect.width, lRect.height / 2f)), propItem.reff, new GUIContent(""));
            //Region 2 (Method Selector & Value Editor)
            var mRect = new Rect(rect.x + rect.width / 3.5f, rect.y, rect.width - rect.width * (1/3.5f + 1/5f), rect.height);
            var objReff = propItem.reff.objectReferenceValue;
            var objReffID = propItem.reff.objectReferenceInstanceIDValue;
            //Method Selector
            var methodSelector = new UnityMethodSelector();
            if (objReff != null)
            {
                var objReffList = new List<UnityEngine.Object>();
                if (objReff is Component)
                {
                    objReffList.Add((objReff as Component).gameObject);
                    objReffList.AddRange((objReff as Component).GetComponents<Component>());
                }
                else if (objReff is GameObject)
                {
                    objReffList.Add(objReff);
                    objReffList.AddRange((objReff as GameObject).GetComponents<Component>());
                }
                methodSelector.targetObjects = objReffList;
                if (!string.IsNullOrEmpty(propItem.methodName.stringValue))
                {
                    methodSelector.selectedItem = new UnityMethodSelectorItem(objReff, null);
                    methodSelector.selectedItem.m_selectedParamType = new Type[] { UnityExUtility.UnityReadableTypeList[propItem.methodType.enumValueIndex - 1] };
                    methodSelector.selectedItem._assignValidMethodByName(propItem.methodName.stringValue);
                }
                methodSelector.onMethodSelected = (m) => { _ms_onMethodSelected(propItem, m); };
                methodSelector.onValidateMenuName = (obj, m) => { return _ms_onValidateMenuName(propItem, obj, m); };
            }
            else if(objReffID == 0){
                propItem.methodName.stringValue = "";
                propItem.paramTypePath.stringValue = "";
                propItem.methodType.enumValueIndex = 0;
            }
            EditorGUI.BeginDisabledGroup(objReff == null);
            methodSelector.Draw(temp_uiMargin.Remove(new Rect(mRect.x, mRect.y, mRect.width, mRect.height / 2f)));
            //Value Editor
            if (methodSelector.selectedItem != null && methodSelector.selectedItem.m_selectedMethod != null) {
                var item = methodSelector.selectedItem;
                var itemMParam = item.m_selectedMethod.GetParameters();
                if (itemMParam.Length > 0) {
                    var itemMParamType = itemMParam[0].ParameterType;
                    EditorGUIX.__drawUnityEventValue(temp_uiMargin.Remove(new Rect(mRect.x, mRect.y + mRect.height / 2f, mRect.width, mRect.height / 2f)), propItem.value, itemMParamType);
                }
            }
            //Region 3 (Delay Value)
            var rRect = new Rect(rect.x + rect.width - rect.width / 5f, rect.y, rect.width / 5f, rect.height);
            EditorGUI.PrefixLabel(temp_uiMargin.Remove(new Rect(rRect.x, rRect.y, rRect.width, rRect.height / 2f)), new GUIContent("Delay"));
            var val = EditorGUI.FloatField(temp_uiMargin.Remove(new Rect(rRect.x, rRect.y + rRect.height / 2f, rRect.width, rRect.height / 2f)), propItem.delay.floatValue);
            if (val < 0)
                val = 0.0f;
            propItem.delay.floatValue = val;
            EditorGUI.EndDisabledGroup();
            EditorGUI.EndDisabledGroup();
        }
        #endregion

        #region MethodSelected Callback
        private void _ms_onMethodSelected(PropItem item, UnityMethodSelectorItem m) {
            if (m == null)
            {
                item.methodName.stringValue = "";
                item.paramTypePath.stringValue = "";
                item.methodType.enumValueIndex = 0;
            }
            else {
                item.methodName.stringValue = m.m_selectedMethod.Name;
                item.reff.objectReferenceValue = m.m_selectedObject;
                var param = m.m_selectedMethod.GetParameters();
                if (param.Length > 0)
                {
                    item.paramTypePath.stringValue = param[0].ParameterType.FullName;
                    item.methodType.enumValueIndex = (int)UnityExUtility.getUnityReadableType(param[0].ParameterType) + 1;
                }
            }
            temp_inspectedProp.serializedObject.ApplyModifiedProperties();
        }
        private string _ms_onValidateMenuName(PropItem item, UnityEngine.Object obj, MethodInfo m) {
            var mName = m.Name;
            var mType = mName.Contains("set_")?"Property":"Method";
            var mParams = m.GetParameters();
            if (mParams.Length > 0) {
                var mParam = mParams[0];
                var mParamType = mParam.ParameterType;
                var mParamTypeName = mParamType.Name;
                switch (mParamTypeName) {
                    case "Single":
                        mParamTypeName = "float";
                        break;
                    case "Int32":
                        mParamTypeName = "int";
                        break;
                    case "Boolean":
                        mParamTypeName = "bool";
                        break;
                    case "String":
                        mParamTypeName = "string";
                        break;
                }
                switch (mType) {
                    case "Property":
                        return obj.GetType().Name + "/" + mType + "/" + mName.Remove(0,4) + "\t" + mParamTypeName;
                    case "Method":
                        return obj.GetType().Name + "/" + mType + "/" + mName + "(" + mParamTypeName + ")";
                }
            }
            return obj.GetType().Name + "/" + mType + "/" + mName + "()";
        }
        #endregion

        class PropItem {
            public SerializedProperty
                enable,
                reff,
                methodName,
                methodType,
                paramTypePath,
                delay,
                value;
            public PropItem(SerializedProperty prop) {
                enable = prop.FindPropertyRelative("m_enable");
                reff = prop.FindPropertyRelative("m_reff");
                methodName = prop.FindPropertyRelative("m_methodName");
                methodType = prop.FindPropertyRelative("m_methodType");
                delay = prop.FindPropertyRelative("m_delay");
                paramTypePath = prop.FindPropertyRelative("m_methodParamTypePath");
                value = prop.FindPropertyRelative("m_value");
            }
        }
    }
}
