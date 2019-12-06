using UnityEngine;
using UnityEditor;
using Imoet.Unity.Events;
using System.Collections.Generic;
using System.Reflection;
using System;

namespace Imoet.UnityEditor
{
    [CustomPropertyDrawer(typeof(ImoetComponentMethod),true)]
    public class ImoetComponentMethodDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return EditorGUIUtility.singleLineHeight;
        }
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var _targetObject = property.FindPropertyRelative("_targetObject");
            var _targetMethodName = property.FindPropertyRelative("_targetMethodName");
            var _targetMethodAsmName = property.FindPropertyRelative("_targetMethodAsmName");
            var _paramAsmName = property.FindPropertyRelative("_paramAsmName");

            Type paramAsmType = null;
            if (_paramAsmName != null)
                paramAsmType = Type.GetType(_paramAsmName.stringValue);

            var rectSizeLeft = EditorGUI.PrefixLabel(position, new GUIContent(property.displayName +"("+ paramAsmType + ")"));

            _targetObject.objectReferenceValue = EditorGUI.ObjectField(
                new Rect(rectSizeLeft.x, rectSizeLeft.y, rectSizeLeft.width/2f, rectSizeLeft.height),
                new GUIContent(""), _targetObject.objectReferenceValue,typeof(Component),true);

            var tgtDrawer = _targetObject.objectReferenceValue;
            var isTgtDrawerComp = tgtDrawer is Component;

            var methodDrawer = new UnityMethodSelector();
            methodDrawer.binding = BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod;
            methodDrawer.maxInspectedParameter = 0;
            
            if (tgtDrawer != null && isTgtDrawerComp) {
                methodDrawer.targetObjects = ((Component)tgtDrawer).GetComponents<Component>();
                methodDrawer.selectedItem = new UnityMethodSelectorItem(_targetObject.objectReferenceValue,null);
                methodDrawer.selectedItem._assignValidMethodByName(_targetMethodName.stringValue, binding: methodDrawer.binding);
            }

            if (_paramAsmName != null && !string.IsNullOrEmpty(_paramAsmName.stringValue)) {
                methodDrawer.maxInspectedParameter = 1;
                methodDrawer.targetMethodParamType = new Type[] { Type.GetType(_paramAsmName.stringValue) };
                methodDrawer.onValidateMethod = (obj, method) => {
                    var methodParams = method.GetParameters();
                    if (methodParams.Length == 1) {
                        var methodParam = methodParams[0];
                        if (methodParam.ParameterType == Type.GetType(_paramAsmName.stringValue))
                            return true;
                    }
                    return false;
                };
                methodDrawer.onValidateMenuName = (obj, method) => {
                    var methodParams = method.GetParameters();
                    var paramNames = new List<string>();
                    if (methodParams.Length == methodDrawer.maxInspectedParameter)
                        foreach (var methodParam in methodParams)
                            paramNames.Add(methodParam.ParameterType.Name);

                    var methodName = method.Name;
                    if (methodName.StartsWith("set_"))
                        methodName = methodName.Replace("set_", "") + "\t" + string.Concat(paramNames.ToArray());
                    else
                        methodName += "(" + string.Concat(paramNames.ToArray()) + ")";
                    
                    return obj.GetType().Name + "/" + methodName;
                };
            }
            else {
                methodDrawer.onValidateMethod = (obj, method) => { return true; };
                methodDrawer.onValidateMenuName = (obj, method) => {
                    return obj.GetType().Name + "/" + method.Name + "()";
                };
            }
            methodDrawer.onMethodSelected = (selectedMethod) =>
            {
                if (selectedMethod != null) {
                    _targetObject.objectReferenceValue = selectedMethod.selectedObject;
                    _targetMethodName.stringValue = selectedMethod.selectedMethod.Name;
                    _targetMethodAsmName.stringValue = selectedMethod.selectedObject.GetType().AssemblyQualifiedName;
                }
                else {
                    _targetMethodName.stringValue = null;
                    _targetMethodAsmName.stringValue = null;
                }
                property.serializedObject.ApplyModifiedProperties();
            };
            methodDrawer.onSelectedMethodShowed = (selectedMethod) =>
            {
                var methodName = selectedMethod.selectedMethod.Name;
                var resultMethodName = (methodName.StartsWith("set_") || methodName.StartsWith("get_")) ? methodName.Replace("set_", "").Replace("get_", "") : methodName + "()" ;
                return selectedMethod.selectedObject.GetType().Name + "." + resultMethodName;
            };
            methodDrawer.Draw(new Rect(rectSizeLeft.x + rectSizeLeft.width / 2f, rectSizeLeft.y, rectSizeLeft.width / 2f, rectSizeLeft.height));
        }
    }
}
