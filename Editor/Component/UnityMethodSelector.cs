//Imoet UnityMethodSelector
//Copyright Yusuf Sulaiman (C) 2019 <yusufxh@ymail.com>
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Imoet.Unity.Utility;
using Type = System.Type;

namespace Imoet.UnityEditor
{
    public sealed class UnityMethodSelector
    {
        /// <summary>
        /// List of object that you want to inspect
        /// </summary>
        public IEnumerable<Object> targetObjects { get; set; }

        /// <summary>
        /// Type of method parameter that you want to inspect
        /// </summary>
        public IEnumerable<Type> targetMethodParamType
        {
            get { return m_inspectedParamType; }
            set { m_inspectedParamType = value; }
        }
        /// <summary>
        /// Type of returned value of method that you want to inspect
        /// </summary>
        public IEnumerable<Type> targetMethodReturnType
        {
            get { return m_inspectedReturnType; }
            set { m_inspectedReturnType = value; }
        }

        /// <summary>
        /// Selected valid MethodItem
        /// </summary>
        public UnityMethodSelectorItem selectedItem { get; set; }

        /// <summary>
        /// Binding rule for search method
        /// </summary>
        public BindingFlags binding
        {
            get { return m_binding; }
            set { m_binding = value; }
        }

        /// <summary>
        /// Maximum Number of inspectedParameter
        /// </summary>
        public int maxInspectedParameter
        {
            get { return m_maxNumParam; }
            set { m_maxNumParam = value; }
        }

        /// <summary>
        /// Callback when valid method has been selected
        /// </summary>
        public System.Action<UnityMethodSelectorItem> onMethodSelected { get; set; }
        /// <summary>
        /// Callback when the class validated the method. this also can be used to determine which method that you want to include to menu pool
        /// </summary>
        public System.Func<Object, MethodInfo, bool> onValidateMethod { get; set; }
        /// <summary>
        /// Callback when the class is going to populate methods for the menu. this also can be used to decide which object that should be included or not
        /// </summary>
        public System.Func<IEnumerable<Object>,IEnumerable<Object>> onPopulateObjects { get; set; }
        /// <summary>
        /// Callback when DropDown menu is about to show. this also can be used to manipulate method name that represented in the menu
        /// </summary>
        public System.Func<Object, MethodInfo, string> onValidateMenuName { get; set; }

        private IEnumerable<Type> m_inspectedParamType = UnityExUtility.UnityReadableTypeList;
        private IEnumerable<Type> m_inspectedReturnType = new Type[] { typeof(void) };
        private int m_maxNumParam = 1;
        private BindingFlags m_binding = BindingFlags.Instance | BindingFlags.Public;
        private GenericMenu m_menu;
        private static Style m_style;

        #region Private Function
        private void _onMenuSelected(object item)
        {
            selectedItem = (UnityMethodSelectorItem)item;
            if (onMethodSelected != null) {
                onMethodSelected(selectedItem);
            }
        }

        private bool _isValid(IEnumerable<Type> checker, Type type)
        {
            foreach (Type t in checker) {
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
                targetObjects = onPopulateObjects(targetObjects);
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
                    valid = _defaultValidatingMethod(obj, objMethodInfo);
                    if (valid && onValidateMethod != null)  {
                        valid = onValidateMethod(obj, objMethodInfo);
                    }
                    if (valid)
                    {
                        var validMethods = new UnityMethodSelectorItem(obj, objMethodInfo);
                        string menuPath = null;
                        if (onValidateMenuName != null)
                        {
                            menuPath = onValidateMenuName(obj, objMethodInfo);
                        }
                        else
                        {
                            menuPath = validMethods.m_selectedObjectType.Name + "/" + validMethods.m_selectedMethod.Name;
                        }

                        m_menu.AddItem(new GUIContent(menuPath), false, _onMenuSelected, validMethods);
                    }
                }
            }
        }
#endregion

        //Public Function
        /// <summary>
        /// Draw the GUI
        /// </summary>
        /// <param name="rect"></param>
        public void Draw(Rect rect)
        {
            if (m_style == null)
            {
                m_style = new Style();
            }

            string buttonName = "None";
            if (selectedItem != null && selectedItem.m_selectedMethod != null)
            {
                string methodName = selectedItem.m_selectedMethod.Name.Replace("set_", "").Replace("get_", "");
                buttonName = selectedItem.m_selectedMethod.DeclaringType.Name + "." + methodName;
            }
            if (GUI.Button(rect, buttonName, m_style.dropDown))
            {
                ShowMenuContext();
            }
        }
        /// <summary>
        /// Forcely Show the Dropdown Menu. this method can be used when you want to re-show the menu when its not valid anymore
        /// </summary>
        public void ShowMenuContext()
        {
            m_menu = new GenericMenu();
            m_menu.AddItem(new GUIContent("None"), false, _onMenuSelected, null);
            m_menu.AddSeparator("");
            _fillMenu();
            m_menu.ShowAsContext();
        }

        private class Style {
            public GUIStyle dropDown = new GUIStyle(EditorStyles.popup);
        }
    }

    /// <summary>
    /// Class that representate the selected method of <see cref="UnityMethodSelector"/>
    /// </summary>
    public sealed class UnityMethodSelectorItem
    {
        internal UnityMethodSelectorItem(Object obj, MethodInfo method)
        {
            m_selectedObject = obj;
            m_selectedMethod = method;
            if (m_selectedObject != null) {
                m_selectedObjectType = m_selectedObject.GetType();
            }
            if (method != null) {
                m_selectedReturnType = method.ReturnParameter.ParameterType;
                ParameterInfo[] param = method.GetParameters();
                int paramInt = param.Length;
                m_selectedParamType = new Type[paramInt];
                for (int i = 0; i < paramInt; i++)
                {
                    m_selectedParamType[i] = param[i].ParameterType;
                }
            }
        }
        internal Object m_selectedObject;
        internal Type m_selectedObjectType;
        internal MethodInfo m_selectedMethod;
        internal Type[] m_selectedParamType;
        internal Type m_selectedReturnType;

        /// <summary>
        /// Selected inspected object
        /// </summary>
        public Object selectedObject { get { return m_selectedObject; } }
        /// <summary>
        /// Selected method of the object
        /// </summary>
        public MethodInfo selectedMethod { get { return m_selectedMethod; } }
        /// <summary>
        /// Selected parameters of method
        /// </summary>
        public Type[] selectedParamType { get { return m_selectedParamType; } }
        /// <summary>
        /// Selected returnType of method
        /// </summary>
        public Type selectedReturnType { get { return m_selectedReturnType; } }

        internal bool _checkValidation(BindingFlags binding = BindingFlags.Instance | BindingFlags.Public)
        {
            if (m_selectedObject == null || m_selectedMethod == null) {
                return false;
            }

            MethodInfo[] objMethodList = m_selectedObjectType.GetMethods(binding);
            foreach (MethodInfo mInfo in objMethodList) {
                if (mInfo == m_selectedMethod) {
                    return true;
                }
            }
            return false;
        }

        internal bool _assignValidMethodByName(string methodName, BindingFlags binding = BindingFlags.Instance | BindingFlags.Public)
        {
            if (m_selectedObject == null || string.IsNullOrEmpty(methodName))
                return false;
            MethodInfo[] obMethodList = m_selectedObjectType.GetMethods(binding);
            MethodInfo _res = null;
            foreach (MethodInfo mInfo in obMethodList)
            {
                if (mInfo.Name == methodName)
                {
                    bool _ok = true;
                    if (m_selectedReturnType != null)
                    {
                        Type returnType = mInfo.ReturnParameter.ParameterType;
                        if ((!m_selectedReturnType.IsEnum && !returnType.IsEnum) &&
                           !m_selectedReturnType.IsAssignableFrom(returnType)) {
                            _ok = false;
                        }
                    }
                    if (m_selectedParamType != null)
                    {
                        ParameterInfo[] paramInfoList = mInfo.GetParameters();
                        int paramInfoListLen = paramInfoList.Length;
                        int selectedParamTypeLen = m_selectedParamType.Length;
                        if (selectedParamTypeLen == paramInfoListLen) {
                            for (int i = 0; i < paramInfoListLen; i++)
                            {
                                if ((!m_selectedParamType[i].IsEnum && !paramInfoList[i].ParameterType.IsEnum) &&
                                    !m_selectedParamType[i].IsAssignableFrom(paramInfoList[i].ParameterType)) {
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
                m_selectedMethod = _res;
            return _res != null;
        }
    }
}