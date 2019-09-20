using UnityEditor;
using UnityEngine;
using System.Reflection;
using Imoet.Unity;
namespace Imoet.UnityEditor
{
    [CustomPropertyDrawer(typeof(WideToggleAttribute))]
    public class WideToggleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                Debug.LogError("Unsupported property type! Wide Button attribute must be used to boolean property");
                return;
            }
            WideToggleAttribute attr = attribute as WideToggleAttribute;
            property.boolValue = EditorGUIX.ToogleWideBar(position, property.boolValue, label, attr.disableText, attr.enableText);
        }
    }
    [CustomPropertyDrawer(typeof(FullBarToggleAttribute))]
    public class FullBarToggleDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                Debug.LogError("Unsupported property type! FullBar Button attribute must be used to boolean property");
                return;
            }
            property.boolValue = EditorGUIX.ToogleFullBar(position, property.boolValue, label);
        }
    }
    [CustomPropertyDrawer(typeof(ExeButtonAttribute))]
    public class ExeButtonDrawer : PropertyDrawer
    {
        private bool isValid = true;
        void OnEnable()
        {
            Debug.Log("Test");
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!isValid)
            {
                return 34;
            }
            else
            {
                return EditorGUI.GetPropertyHeight(property);
            }
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.Boolean)
            {
                Debug.LogError("Unsupported property type! ExeButton must be used to boolean property");
                return;
            }
            ExeButtonAttribute attr = attribute as ExeButtonAttribute;
            if (!string.IsNullOrEmpty(attr.methodName))
            {
                Object obj = property.serializedObject.targetObject;
                MethodInfo mInfo = obj.GetType().GetMethod(attr.methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                if (mInfo != null)
                {
                    ParameterInfo[] paramInfo = mInfo.GetParameters();
                    if (paramInfo.Length <= 0)
                    {
                        isValid = true;
                        if (GUI.Button(position, property.displayName + " (" + attr.methodName + ")"))
                        {
                            mInfo.Invoke(obj, null);
                        }
                    }
                    else
                    {
                        EditorGUI.HelpBox(position, "Only Accepting Non Parameter Method (" + attr.methodName + ")", MessageType.Error);
                        isValid = false;
                        return;
                    }
                }
                else
                {
                    EditorGUI.HelpBox(position, "Cannot Find Method: " + attr.methodName, MessageType.Error);
                    isValid = false;
                    return;
                }
            }
        }
    }
    [CustomPropertyDrawer(typeof(ShowIfNotNullAttribute))]
    public class ShowIfNullAttributeEditor : PropertyDrawer
    {
        bool isShown;
        bool isInspected;
        public bool Proccess(SerializedProperty serializedProperty)
        {
            ShowIfNotNullAttribute attr = (ShowIfNotNullAttribute)attribute;
            SerializedProperty inspectedProperty = serializedProperty.serializedObject.FindProperty(attr.inspectedPropertyName);
            if (inspectedProperty != null && !inspectedProperty.Equals(serializedProperty))
            {
                SerializedPropertyType propType = inspectedProperty.propertyType;
                object value = default(object);
                switch (propType)
                {
                    case SerializedPropertyType.String:
                        value = inspectedProperty.stringValue;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.ObjectReference:
                        value = inspectedProperty.objectReferenceValue;
                        isInspected = true;
                        break;
                }
                if (isInspected)
                {
                    if (attr.reverse)
                        return value == null;
                    else
                        return value != null;
                }
            }
            return false;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            isShown = Proccess(property);
            if (isInspected && !isShown)
            {
                return 0;
            }
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (isInspected && !isShown)
            {
                return;
            }
            EditorGUI.PropertyField(position, property);
        }
    }
    [CustomPropertyDrawer(typeof(ShowIfMoreThanAttribute))]
    public class ShowIfMoreThanAttributeEditor : PropertyDrawer
    {
        bool isShown;
        bool isInspected;
        public bool Proccess(SerializedProperty serializedProperty)
        {
            ShowIfMoreThanAttribute attr = (ShowIfMoreThanAttribute)attribute;
            SerializedProperty inspectedProperty = serializedProperty.serializedObject.FindProperty(attr.inspectedPropertyName);
            if (inspectedProperty != null && !inspectedProperty.Equals(serializedProperty))
            {
                SerializedPropertyType propType = inspectedProperty.propertyType;
                if (propType == SerializedPropertyType.Float)
                {
                    float value = inspectedProperty.floatValue;
                    isInspected = true;
                    return value > attr.num;
                }
                else if (propType == SerializedPropertyType.Integer)
                {
                    int value = inspectedProperty.intValue;
                    isInspected = true;
                    return value > attr.num;
                }
            }
            return false;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            isShown = Proccess(property);
            if (isInspected && !isShown)
            {
                return 0;
            }
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (isInspected && !isShown)
            {
                return;
            }
            EditorGUI.PropertyField(position, property);
        }
    }
    [CustomPropertyDrawer(typeof(ShowIfLessThanAttribute))]
    public class ShowIfLessThanAttributeEditor : PropertyDrawer
    {
        bool isShown;
        bool isInspected;
        public bool Proccess(SerializedProperty serializedProperty)
        {
            ShowIfLessThanAttribute attr = (ShowIfLessThanAttribute)attribute;
            SerializedProperty inspectedProperty = serializedProperty.serializedObject.FindProperty(attr.inspectedPropertyName);
            if (inspectedProperty != null && !inspectedProperty.Equals(serializedProperty))
            {
                SerializedPropertyType propType = inspectedProperty.propertyType;
                if (propType == SerializedPropertyType.Float)
                {
                    float value = inspectedProperty.floatValue;
                    isInspected = true;
                    return value < attr.num;
                }
                else if (propType == SerializedPropertyType.Integer)
                {
                    int value = inspectedProperty.intValue;
                    isInspected = true;
                    return value < attr.num;
                }
            }
            return false;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            isShown = Proccess(property);
            if (isInspected && !isShown)
            {
                return 0;
            }
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (isInspected && !isShown)
            {
                return;
            }
            EditorGUI.PropertyField(position, property);
        }
    }
    [CustomPropertyDrawer(typeof(ShowIfInBeetwenAttribute))]
    public class ShowIfInBeetwenAttributeEditor : PropertyDrawer
    {
        bool isShown;
        bool isInspected;
        public bool Proccess(SerializedProperty serializedProperty)
        {
            ShowIfInBeetwenAttribute attr = (ShowIfInBeetwenAttribute)attribute;
            SerializedProperty inspectedProperty = serializedProperty.serializedObject.FindProperty(attr.inspectedPropertyName);
            if (inspectedProperty != null && !inspectedProperty.Equals(serializedProperty))
            {
                SerializedPropertyType propType = inspectedProperty.propertyType;
                if (propType == SerializedPropertyType.Float)
                {
                    float value = inspectedProperty.floatValue;
                    if (value > attr.startNum && value < attr.endNum)
                    {
                        isInspected = true;
                        if (attr.reverse)
                            return false;
                        else
                            return true;
                    }
                }
                else if (propType == SerializedPropertyType.Integer)
                {
                    int value = inspectedProperty.intValue;
                    if (value > attr.startNum && value < attr.endNum)
                    {
                        isInspected = true;
                        if (attr.reverse)
                            return false;
                        else
                            return true;
                    }
                }
            }
            return false;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            isShown = Proccess(property);
            if (isInspected && !isShown)
            {
                return 0;
            }
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (isInspected && !isShown)
            {
                return;
            }
            EditorGUI.PropertyField(position, property);
        }
    }
    [CustomPropertyDrawer(typeof(ShowIfEqualAttribute))]
    public class ShowIfEqualAttributeEditor : PropertyDrawer
    {
        bool isShown;
        bool isInspected;
        public bool Proccess(SerializedProperty serializedProperty)
        {
            ShowIfEqualAttribute attr = (ShowIfEqualAttribute)attribute;
            SerializedProperty inspectedProperty = serializedProperty.serializedObject.FindProperty(attr.inspectedPropertyName);
            if (inspectedProperty != null && !inspectedProperty.Equals(serializedProperty))
            {
                SerializedPropertyType propType = inspectedProperty.propertyType;
                object value = default(object);
                switch (propType)
                {
                    case SerializedPropertyType.Boolean:
                        value = inspectedProperty.boolValue;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.Bounds:
                        value = inspectedProperty.boundsValue;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.Color:
                        value = inspectedProperty.colorValue;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.Integer:
                        value = inspectedProperty.intValue;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.Quaternion:
                        value = inspectedProperty.quaternionValue;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.Float:
                        value = inspectedProperty.floatValue;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.String:
                        value = inspectedProperty.stringValue;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.Rect:
                        value = inspectedProperty.rectValue;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.Vector2:
                        value = inspectedProperty.vector2Value;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.Vector3:
                        value = inspectedProperty.vector3Value;
                        isInspected = true;
                        break;
                    case SerializedPropertyType.Vector4:
                        value = inspectedProperty.vector4Value;
                        isInspected = true;
                        break;
                }
                if (isInspected)
                {
                    if (attr.reverse)
                        return !value.Equals(attr.obj);
                    else
                        return value.Equals(attr.obj);
                }
            }
            return false;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            isShown = Proccess(property);
            if (isInspected && !isShown)
            {
                return 0;
            }
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (isInspected && !isShown){
                return;
            }
            EditorGUI.PropertyField(position, property);
        }
    }

    [CustomPropertyDrawer(typeof(DisabledOnPlayModeAttribute))]
    public class DisabledOnPlayModeAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUIX.DisabledProperty(position, property, EditorApplication.isPlayingOrWillChangePlaymode);
        }
    }
    [CustomPropertyDrawer(typeof(NotEditableAttribute))]
    public class NotEditableAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUIX.DisabledProperty(position, property, true);
        }
    }
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, true);
            EditorGUI.EndDisabledGroup();
        }
    }
    [CustomPropertyDrawer(typeof(BoxInfoAttribute))]
    public class BoxInfoAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, new GUIContent(property.displayName), true);
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
    [CustomPropertyDrawer(typeof(BoxWarningAttribute))]
    public class BoxWarningAttributeDrawer : PropertyDrawer
    {

    }
    [CustomPropertyDrawer(typeof(BoxErrorAttribute))]
    public class BoxErrorAttributeDrawer : PropertyDrawer
    {

    }

}