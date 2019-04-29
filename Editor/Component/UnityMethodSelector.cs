using Imoet.Unity.Events;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Type = System.Type;

namespace Imoet.UnityEditor
{
    public sealed class UnityMethodSelector
    {
        public IEnumerable<Object> targetObjects { get; set; }

        public IEnumerable<Type> targetMethodParamType
        {
            get { return m_inspectedParamType; }
            set { m_inspectedParamType = value; }
        }

        public IEnumerable<Type> targetMethodReturnType
        {
            get { return m_inspectedReturnType; }
            set { m_inspectedReturnType = value; }
        }

        public UnityMethodItem selectedItem { get; set; }

        public BindingFlags binding
        {
            get { return m_binding; }
            set { m_binding = value; }
        }

        public int maxInspectedParameter
        {
            get { return m_maxNumParam; }
            set { m_maxNumParam = value; }
        }

        public System.Action<UnityMethodItem> onMethodSelected { get; set; }
        public System.Func<Object, MethodInfo, bool> onValidateMethod { get; set; }
        public System.Func<Object[]> onPopulateObjects { get; set; }
        public System.Func<Object, MethodInfo, string> onValidateMenuName { get; set; }

        private IEnumerable<Type> m_inspectedParamType = UnityEventExUtility.UnityReadableTypeList;
        private IEnumerable<Type> m_inspectedReturnType = new Type[] { typeof(void) };
        private int m_maxNumParam = 1;
        private BindingFlags m_binding = BindingFlags.Instance | BindingFlags.Public;
        private GenericMenu m_menu;
        private static Style m_style;

        //Private Function
        private void _onMenuSelected(object item)
        {
            selectedItem = (UnityMethodItem)item;
            if (onMethodSelected != null)
            {
                onMethodSelected(selectedItem);
            }
        }

        private bool _isValid(IEnumerable<Type> checker, Type type)
        {
            foreach (Type t in checker)
            {
                if (t.IsAssignableFrom(type))
                {
                    return true;
                }
            }
            return false;
        }

        private bool _defaultValidatingMethod(Object obj, MethodInfo method)
        {
            if (method.DeclaringType == obj.GetType() && 
                method.GetCustomAttributes(typeof(System.ObsoleteAttribute), true).Length == 0)
            {
                Type objMethodReturnType = method.ReturnParameter.ParameterType;
                if (_isValid(m_inspectedReturnType, objMethodReturnType))
                {
                    ParameterInfo[] objMethodParams = method.GetParameters();
                    if (objMethodParams.Length <= m_maxNumParam)
                    {
                        foreach (ParameterInfo param in objMethodParams)
                        {
                            if (!_isValid(m_inspectedParamType, param.ParameterType))
                            {
                                return false;
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        private void _fillMenu()
        {
            if (onPopulateObjects != null)
            {
                targetObjects = onPopulateObjects();
            }

            if (targetObjects == null)
            {
                return;
            }

            foreach (Object obj in targetObjects)
            {
                Type objType = obj.GetType();
                MethodInfo[] objMethods = objType.GetMethods(m_binding);
                foreach (MethodInfo objMethodInfo in objMethods)
                {
                    bool valid = false;
                    if (onValidateMethod != null)
                    {
                        valid = onValidateMethod(obj, objMethodInfo);
                    }
                    else
                    {
                        valid = _defaultValidatingMethod(obj, objMethodInfo);
                    }

                    if (valid)
                    {
                        var validMethods = new UnityMethodItem(obj, objMethodInfo);
                        string menuPath = null;
                        if (onValidateMenuName != null)
                        {
                            menuPath = onValidateMenuName(obj, objMethodInfo);
                        }
                        else
                        {
                            menuPath = validMethods.selectedObjectType.Name + "/" + validMethods.selectedMethod.Name;
                        }

                        m_menu.AddItem(new GUIContent(menuPath), false, _onMenuSelected, validMethods);
                    }
                }
            }
        }
        //Public Function
        public void Draw(Rect rect)
        {
            if (m_style == null)
            {
                m_style = new Style();
            }

            string buttonName = "None";
            if (selectedItem != null && selectedItem.selectedMethod != null)
            {
                string methodName = selectedItem.selectedMethod.Name.Replace("set_", "").Replace("get_", "");
                buttonName = selectedItem.selectedMethod.DeclaringType.Name + "." + methodName;
            }
            if (GUI.Button(rect, buttonName, m_style.dropDown))
            {
                ShowMenuContext();
            }
        }

        public void ShowMenuContext()
        {
            m_menu = new GenericMenu();
            m_menu.AddItem(new GUIContent("None"), false, _onMenuSelected, null);
            m_menu.AddSeparator("");
            _fillMenu();
            m_menu.ShowAsContext();
        }

        private class Style
        {
            public GUIStyle dropDown = new GUIStyle(EditorStyles.popup);
        }
    }

    public sealed class UnityMethodItem
    {
        public UnityMethodItem(Object obj, MethodInfo method)
        {
            selectedObject = obj;
            selectedMethod = method;
            if (selectedObject != null)
            {
                selectedObjectType = selectedObject.GetType();
            }
            if (method != null)
            {
                selectedReturnType = method.ReturnParameter.ParameterType;
                ParameterInfo[] param = method.GetParameters();
                int paramInt = param.Length;
                selectedParamType = new Type[paramInt];
                for (int i = 0; i < paramInt; i++)
                {
                    selectedParamType[i] = param[i].ParameterType;
                }
            }
        }
        public Object selectedObject;
        public Type selectedObjectType;
        public MethodInfo selectedMethod;
        public Type[] selectedParamType;
        public Type selectedReturnType;

        public bool CheckValidation(BindingFlags binding = BindingFlags.Instance | BindingFlags.Public)
        {
            if (selectedObject == null || selectedMethod == null)
            {
                return false;
            }

            MethodInfo[] objMethodList = selectedObjectType.GetMethods(binding);
            foreach (MethodInfo mInfo in objMethodList)
            {
                if (mInfo == selectedMethod)
                {
                    return true;
                }
            }
            return false;
        }

        public bool AssignValidMethodByName(string methodName, BindingFlags binding = BindingFlags.Instance | BindingFlags.Public)
        {
            
            if (selectedObject == null || string.IsNullOrEmpty(methodName))
                return false;
            MethodInfo[] obMethodList = selectedObjectType.GetMethods(binding);
            MethodInfo _res = null;
            foreach (MethodInfo mInfo in obMethodList)
            {
                if (mInfo.Name == methodName)
                {
                    bool _ok = true;
                    if (selectedReturnType != null)
                    {
                        Type returnType = mInfo.ReturnParameter.ParameterType;
                        if ((!selectedReturnType.IsEnum && !returnType.IsEnum) &&
                           !selectedReturnType.IsAssignableFrom(returnType)) {
                            _ok = false;
                        }
                    }
                    if (selectedParamType != null)
                    {
                        ParameterInfo[] paramInfoList = mInfo.GetParameters();
                        int paramInfoListLen = paramInfoList.Length;
                        int selectedParamTypeLen = selectedParamType.Length;
                        if (selectedParamTypeLen == paramInfoListLen) {
                            for (int i = 0; i < paramInfoListLen; i++)
                            {
                                if ((!selectedParamType[i].IsEnum && !paramInfoList[i].ParameterType.IsEnum) &&
                                    !selectedParamType[i].IsAssignableFrom(paramInfoList[i].ParameterType)) {
                                    _ok = false;
                                }
                            }
                        }
                    }
                    if (_ok) {
                        _res = mInfo;
                        break;
                    }
                }
            }
            if (_res != null)
                selectedMethod = _res;
            return _res != null;
        }
    }
}