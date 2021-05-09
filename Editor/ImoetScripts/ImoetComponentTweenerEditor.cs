using Imoet.Unity.Animation;
using UnityEditor;
using System.Collections.Generic;
using UnityEngine;
using Imoet.Unity.Utility;

namespace Imoet.UnityEditor
{
    //[CustomEditor(typeof(ImoetComponentTweener))]
    //public class ImoetComponentTweenerEditor : Editor, IDynamicListHeaderDrawer, IDynamicListItemBodyDrawer, IDynamicListItemHeaderDrawer
    //{
    //    #region Private Field
    //    private SerializedProperty m_items;
    //    private SerializedProperty m_autoPlay;
    //    private DynamicList m_list;
    //    private List<Item> m_cacheItem;

    //    private static Style m_style;

    //    internal static readonly System.Type[] __readedType =
    //        new System.Type[] {
    //            typeof(byte),
    //            typeof(short),
    //            typeof(int),
    //            typeof(float),
    //            typeof(double),
    //            typeof(Vector2),
    //            typeof(Vector3),
    //            typeof(Vector4),
    //            typeof(Quaternion),
    //            typeof(Rect),
    //            typeof(Color),
    //            typeof(Color32)
    //        };
    //    #endregion

    //    #region Unity Drawer
    //    private void OnEnable()
    //    {
    //        m_cacheItem = new List<Item>();
    //        m_items = serializedObject.FindProperty("m_items");
    //        m_autoPlay = serializedObject.FindProperty("m_autoPlay");

    //        m_list = new DynamicList(m_items);
    //        m_list.drawerHeader = this;
    //        m_list.drawerItemBody = this;
    //        m_list.drawerItemHeader = this;
    //        m_list.reorderable = false;
    //    }
    //    public override void OnInspectorGUI()
    //    {
    //        if (m_style == null)
    //            m_style = new Style();
    //        EditorGUILayout.PropertyField(m_autoPlay);
    //        m_list.Draw();
    //        serializedObject.ApplyModifiedProperties();
    //        _cleanCache();
    //    }
    //    #endregion

    //    #region Drawer Interface
    //    public float GetHeaderHeight(SerializedProperty property) {
    //        return EditorGUI.GetPropertyHeight(property, new GUIContent(property.displayName));
    //    }

    //    public void DrawHeader(Rect rect, SerializedProperty property) {
    //        GUI.Label(rect, "Components", m_style.headerText);
    //    }

    //    public float GetItemBodyHeight(SerializedProperty property)
    //    {
    //        if (property.isExpanded && m_items.arraySize > 0) {
    //            var item = _findItem(property);
    //            if (item != null && item.m_selector.selectedItem != null) {
    //                return 80f + EditorGUI.GetPropertyHeight(item.m_setting);
    //            }
    //            else
    //                return 22f;
    //        }
    //        return 0;
    //    }

    //    public void DrawItemBody(Rect rect, SerializedProperty property)
    //    {
    //        var singleLine = EditorGUIUtility.singleLineHeight;
    //        //Draw MethodSelector
    //        var item = _findItem(property);
    //        if (item == null)
    //        {
    //            item = new Item(property);
    //            item.parent = this;
    //            m_cacheItem.Add(item);
    //        }
    //        item.m_selector.Draw(new Rect(rect.x, rect.y, rect.width, 17f));
    //        var selectedItem = item.m_selector.selectedItem;
    //        if (selectedItem != null) {
    //            //Draw Parameter Editor
    //            _drawProperty(new Rect(rect.x, rect.y + 17f, rect.width, 17f * 2f + 2f), property, selectedItem);
    //            //Draw Reset Button
    //            if (GUI.Button(new Rect(rect.x, rect.y + 17f * 3 + 3, rect.width, 17f), "Reset Value"))
    //                _applyDefaultValueToProp(property, item.m_selector.selectedItem);
    //            //Draw Tween Setting
    //            EditorGUI.PropertyField(new Rect(rect.x, rect.y + 17f * 4 + 3, rect.width, EditorGUI.GetPropertyHeight(item.m_setting)), item.m_setting, true);
    //        }
    //    }

    //    public float GetItemHeaderHeight(SerializedProperty property) {
    //        return m_list.GetHeaderHeight(property);
    //    }

    //    public void DrawItemHeader(Rect rect, SerializedProperty property) {
    //        var name = property.displayName;
    //        var item = _findItem(property);
    //        if (item != null) {
    //            var selectedItem = item.m_selector.selectedItem;
    //            string methodName = item.m_methodName.stringValue.Replace("set_", "").Replace("get_", "");
    //            if (selectedItem != null) {
    //                name = selectedItem.m_selectedMethod.DeclaringType.Name + "." + methodName;
    //            }
    //            //else {
    //            //    if (!item.m_component.objectReferenceValue && item.m_component.objectReferenceInstanceIDValue == 0)
    //            //        name = "<Missing Component>." + methodName;
    //            //}
    //        }
    //        if (m_list.reorderable)
    //            GUI.Label(rect, name, m_style.labelText);
    //        else
    //        {
    //            if (GUI.Button(rect, name, m_style.labelText))
    //                property.isExpanded = !property.isExpanded;
    //        }
    //    }
    //    #endregion

    //    #region Private Method
    //    private Item _findItem(SerializedProperty property)
    //    {
    //        foreach (var item in m_cacheItem) {
    //            if (item.m_prop == property)
    //                return item;
    //        }
    //        return null;
    //    }
    //    private void _drawProperty(Rect rect, SerializedProperty property, UnityMethodSelectorItem item) {
    //        var prop = _getFixedProperty(property, item);
    //        if (prop == null)
    //            return;

    //        var propStart = prop.FindPropertyRelative("valStart");
    //        var propEnd = prop.FindPropertyRelative("valEnd");
    //        EditorGUILayoutX.BeginWideGUI();
    //        EditorGUIX.__drawCustomUnityValue(new Rect(rect.x, rect.y, rect.width, 17f), propStart, item.selectedParamType[0], "Start");
    //        EditorGUIX.__drawCustomUnityValue(new Rect(rect.x, rect.y + rect.height - 17f, rect.width, 17f), propEnd, item.selectedParamType[0], "End");
    //        EditorGUILayoutX.EndWideGUI();
    //    }

    //    private void _applyValueToProp(object value, System.Type type, SerializedProperty prop) {
    //        var unityType = UnityExUtility.getUnityReadableType(type);
    //        switch (unityType) {
    //            case UnityExUtility.UnityReadableType.Boolean:
    //                prop.boolValue = (bool)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Byte:
    //                prop.intValue = (byte)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Short:
    //                prop.intValue = (short)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Int:
    //                prop.intValue = (int)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Float:
    //                prop.floatValue = (float)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Double:
    //                prop.doubleValue = (double)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Vector2:
    //                prop.vector2Value = (Vector2)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Vector3:
    //                prop.vector3Value = (Vector3)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Vector4:
    //                prop.vector4Value = (Vector4)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Quaternion:
    //                prop.quaternionValue = (Quaternion)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Rect:
    //                prop.rectValue = (Rect)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Color:
    //                prop.colorValue = (Color)value;
    //                break;
    //            case UnityExUtility.UnityReadableType.Color32:
    //                prop.colorValue = (Color32)value;
    //                break;
    //        }
    //    }

    //    private void _applyDefaultValueToProp(SerializedProperty prop, UnityMethodSelectorItem methodItem) {
    //        var tgtProp = _getFixedProperty(prop, methodItem);
    //        var propStart = tgtProp.FindPropertyRelative("valStart");
    //        var propEnd = tgtProp.FindPropertyRelative("valEnd");

    //        var selectedObjType = methodItem.selectedObject.GetType();
    //        var selectedObjMethodName = methodItem.selectedMethod.Name;
    //        if (selectedObjMethodName.Contains("set_")) {
    //            var selectedObjMethod = selectedObjType.GetMethod(methodItem.selectedMethod.Name.Replace("set_", "get_"));
    //            var defaultValue = selectedObjMethod.Invoke(methodItem.selectedObject, null);

    //            _applyValueToProp(defaultValue, defaultValue.GetType(), propStart);
    //            _applyValueToProp(defaultValue, defaultValue.GetType(), propEnd);
    //        }
    //    }


    //    private SerializedProperty _getFixedProperty(SerializedProperty property,UnityMethodSelectorItem item) {
    //        if (item == null)
    //            return null;
    //        var selectedType = UnityExUtility.getUnityReadableType(item.selectedParamType[0]);
    //        if (selectedType <= (int)UnityExUtility.UnityReadableType.Void)
    //            return null;

    //        SerializedProperty resProp = null;
    //        string tLower = "";
    //        var paramType = item.selectedParamType[0];
    //        switch (paramType.Name)
    //        {
    //            case "Single":
    //                tLower = "float";
    //                break;

    //            case "Int32":
    //                tLower = "int";
    //                break;

    //            case "Boolean":
    //                tLower = "bool";
    //                break;

    //            case "String":
    //                tLower = "string";
    //                break;

    //            default:
    //                tLower = paramType.Name;
    //                break;
    //        }
    //        tLower = tLower.ToLower();
    //        resProp = property.FindPropertyRelative("val_"+tLower);

    //        return resProp;
    //    }

    //    private void _cleanCache()
    //    {
    //        for (int i = 0; i < m_cacheItem.Count; i++)
    //        {
    //            if (m_cacheItem[i].m_prop == null)
    //                m_cacheItem.RemoveAt(i);
    //        }
    //    }
    //    #endregion

    //    #region Klass
    //    class Item
    //    {
    //        public ImoetComponentTweenerEditor parent;
    //        public SerializedProperty
    //        //Serialized Field
    //        m_setting,
    //        m_methodName,
    //        m_component,
    //        m_valueType,
    //        m_prop;
    //        public UnityMethodSelector m_selector;
    //        public Item(SerializedProperty prop)
    //        {
    //            m_prop = prop;
    //            //Assign Property
    //            m_setting = prop.FindPropertyRelative("m_setting");
    //            m_methodName = prop.FindPropertyRelative("m_methodName");
    //            m_component = prop.FindPropertyRelative("m_component");
    //            m_valueType = prop.FindPropertyRelative("m_valueType");

    //            //Create Filtered Components to inspect
    //            var obj = (ImoetComponentTweener)prop.serializedObject.targetObject;
    //            var comps = obj.GetComponents<Component>();
    //            var sortedComps = new List<Component>();
    //            var objType = typeof(ImoetComponentTweener);
    //            foreach (var comp in comps) {
    //                if (comp is ImoetComponentTweener || comp is Transform)
    //                    continue;
    //                else
    //                    sortedComps.Add(comp);
    //            }
    //            var sortedCompsArr = sortedComps.ToArray();

    //            //Create Method Selector
    //            m_selector = new UnityMethodSelector();
    //            m_selector.targetObjects = sortedCompsArr;
    //            m_selector.targetMethodParamType = __readedType;
    //            m_selector.maxInspectedParameter = 1;
    //            m_selector.onMethodSelected = (methodItem) => {
    //                if (methodItem == null)
    //                {
    //                    m_methodName.stringValue = "";
    //                    m_component.objectReferenceValue = null;
    //                    m_valueType.enumValueIndex = 0;
    //                }
    //                else {
    //                    m_methodName.stringValue = methodItem.selectedMethod.Name;
    //                    m_component.objectReferenceValue = methodItem.selectedObject;
    //                    m_valueType.enumValueIndex = (int)UnityExUtility.getUnityReadableType(methodItem.selectedParamType[0]) + 1;
    //                    parent._applyDefaultValueToProp(m_prop, methodItem);
    //                }
    //                prop.serializedObject.ApplyModifiedProperties();
    //            };
    //            m_selector.onValidateMethod = (vObj,method) => {
    //                var methodParams = method.GetParameters();
    //                return methodParams.Length != 0;
    //            };
    //            m_selector.onValidateMenuName = (vObj, method) =>
    //            {
    //                var objT = vObj.GetType();
    //                var mName = method.Name;
    //                var mType = mName.Contains("set_") ? "Property" : "Method";
    //                var mParams = method.GetParameters();
    //                if (mParams.Length > 0)
    //                {
    //                    var mParam = mParams[0];
    //                    var mParamType = mParam.ParameterType;
    //                    var mParamTypeName = mParamType.Name;
    //                    switch (mParamTypeName)
    //                    {
    //                        case "Single":
    //                            mParamTypeName = "float";
    //                            break;
    //                        case "Int32":
    //                            mParamTypeName = "int";
    //                            break;
    //                        case "Boolean":
    //                            mParamTypeName = "bool";
    //                            break;
    //                        case "String":
    //                            mParamTypeName = "string";
    //                            break;
    //                    }
    //                    switch (mType)
    //                    {
    //                        case "Property":
    //                            return objT.Name + "/" + mType + "/" + mName.Remove(0, 4) + "\t" + mParamTypeName;
    //                        case "Method":
    //                            return objT.Name + "/" + mType + "/" + mName + "(" + mParamTypeName + ")";
    //                    }
    //                }
    //                return objT.Name + "/" + mType + "/" + mName + "()";
    //            };

    //            if (!string.IsNullOrEmpty(m_methodName.stringValue) && m_component.objectReferenceValue) {
    //                m_selector.selectedItem = new UnityMethodSelectorItem(m_component.objectReferenceValue, null);
    //                m_selector.selectedItem.m_selectedParamType = new System.Type[] { UnityExUtility.UnityReadableTypeList[m_valueType.enumValueIndex - 1] };
    //                m_selector.selectedItem._assignValidMethodByName(m_methodName.stringValue);
    //            }
    //        }
    //    }
    //    class Style
    //    {
    //        public GUIStyle headerText = UnityEditorSkin.midBoldLabel;  
    //        public GUIStyle labelText = new GUIStyle();
    //        public Style() {
    //            labelText.alignment = TextAnchor.MiddleCenter;
    //        }
    //    }
    //    #endregion
    //}
}