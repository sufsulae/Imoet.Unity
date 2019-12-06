#pragma warning disable
using System;
using UnityEngine;
using System.Reflection;
using Imoet.Unity.Utility;
using UnityReadableType = Imoet.Unity.Utility.UnityExUtility.UnityReadableType;

namespace Imoet.Unity.Events
{
    [System.Serializable]
    public class ImoetUnityEventMoethod
    {
        [SerializeField]
        private bool m_enable = true;
        [SerializeField]
        private UnityEngine.Object m_reff;
        [SerializeField]
        internal string m_methodName;
        [SerializeField]
        private UnityReadableType m_methodType;
        [SerializeField]
        private string m_methodParamTypePath;
        [SerializeField]
        private float m_delay;
        [SerializeField]
        internal bool m_executed;
        [SerializeField]
        private UnityExUtility.UnityReadableValue m_value;

        //Public Property
        public bool enable
        {
            get { return m_enable; }
            set { m_enable = value; }
        }
        public UnityEngine.Object inspectedObject
        {
            get { return m_reff; }
        }
        public string inspectedMethod
        {
            get { return m_methodName; }
        }
        public UnityReadableType inspectedMethodType
        {
            get { return m_methodType; }
        }
        public float delay
        {
            get { return m_delay; }
            set
            {
                if (value < 0)
                    m_delay = 0;
                else
                    m_delay = value;
            }
        }

        //Internal Property
        internal bool isExecuted {
            get { return m_executed; }
        }
        internal UnityEngine.Object internalObject {
            get { return m_reff; }
            set { m_reff = value; }
        }
        //Internal Field
        internal MethodInfo internalMethod { get; set; }
        internal ParameterInfo internalMethodParam { get; set; }
        //Private Field
        private Type m_methodDeclType;

        public void Invoke()
        {
            if (SetupMethod())
                TryExecute();
        }
        public bool SetupMethod()
        {
            //We Dont have any method, Find It!
            if (m_reff != null)
            {
                MethodInfo[] allMethodInfo = m_reff.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.InvokeMethod);
                foreach (MethodInfo mInfo in allMethodInfo)
                {
                    if (mInfo.Name == m_methodName)
                    {
                        if (m_methodType != UnityReadableType.Void)
                        {
                            ParameterInfo[] mInfoParams = mInfo.GetParameters();
                            if (mInfoParams.Length == 1)
                            {
                                Type mInfoParamType = mInfoParams[0].ParameterType;

                                if (
                                    mInfoParams[0].ParameterType == typeof(Enum) && 
                                    mInfoParams[0].ParameterType.FullName == m_methodParamTypePath ||
                                    UnityExUtility.UnityReadableTypeList[(int)m_methodType].IsAssignableFrom(mInfoParams[0].ParameterType) &&
                                     mInfoParamType.FullName == m_methodParamTypePath)
                                {
                                    internalMethodParam = mInfoParams[0];
                                    internalMethod = mInfo;
                                    return true;
                                }
                            }
                        }
                        else
                        {
                            internalMethod = mInfo;
                            internalMethodParam = null;
                            return true;
                        }
                    }
                }
            }
            //m_method = null;
            //m_methodParam = null;
            return false;
        }

        private bool TryExecute()
        {
            if (internalMethod != null && internalMethodParam == null)
            {
                internalMethod.Invoke(m_reff, null);
                return true;
            }
            else
            {
                UnityExUtility.UnityReadableType readableType = UnityExUtility.getUnityReadableType(internalMethodParam.ParameterType);
                switch (readableType)
                {
                    case UnityExUtility.UnityReadableType.Boolean:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_bool });
                        return true;
                    case UnityExUtility.UnityReadableType.Byte:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_byte });
                        return true;
                    case UnityExUtility.UnityReadableType.Color:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_color });
                        return true;
                    case UnityExUtility.UnityReadableType.Color32:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_color32 });
                        return true;
                    case UnityExUtility.UnityReadableType.Double:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_double });
                        return true;
                    case UnityExUtility.UnityReadableType.Enum:
                        string[] allNames = Enum.GetNames(internalMethodParam.ParameterType);
                        internalMethod.Invoke(m_reff, new object[] { Enum.Parse(internalMethodParam.ParameterType, allNames[m_value.m_enum]) });
                        return true;
                    case UnityExUtility.UnityReadableType.Float:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_float });
                        return true;
                    case UnityExUtility.UnityReadableType.Int:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_int });
                        return true;
                    case UnityExUtility.UnityReadableType.Quaternion:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_quaternion });
                        return true;
                    case UnityExUtility.UnityReadableType.Rect:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_rect });
                        return true;
                    case UnityExUtility.UnityReadableType.Short:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_short });
                        return true;
                    case UnityExUtility.UnityReadableType.String:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_string });
                        return true;
                    case UnityExUtility.UnityReadableType.UnityObject:
                        if (m_value.m_unityObject != null)
                            internalMethod.Invoke(m_reff, new object[] { m_value.m_unityObject });
                        else
                            internalMethod.Invoke(m_reff, new object[] { null });
                        return true;
                    case UnityExUtility.UnityReadableType.Vector2:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_vector2 });
                        return true;
                    case UnityExUtility.UnityReadableType.Vector3:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_vector3 });
                        return true;
                    case UnityExUtility.UnityReadableType.Vector4:
                        internalMethod.Invoke(m_reff, new object[] { m_value.m_vector4 });
                        return true;
                }
            }
            return false;
        }
    }

}
