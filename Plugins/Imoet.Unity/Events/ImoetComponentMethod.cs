using Imoet.Unity.Utility;
using System.Reflection;
using UnityEngine;

namespace Imoet.Unity.Events
{
    [System.Serializable]
    public class ImoetComponentMethod
    {
        [SerializeField]
        protected Object _targetObject;
        [SerializeField]
        protected string _targetMethodName;
        [SerializeField]
        protected string _targetMethodAsmName;
        [SerializeField]
        protected string _paramAsmName;

        public bool isValid { get { return _isValid; } }
        public Object targetObject { get { return _targetObject; } }
        public MethodInfo targetMethod { get { return _method; } }
        public Assembly assembly { get { return _assemblyInfo; } }

        protected MethodInfo _method;
        protected Assembly _assemblyInfo;
        protected bool _isValid = false;

        public virtual void Validate() {
            _isValid = false;
            _method = null;
            _assemblyInfo = null;

            //Ignore it if the field is empty / null
            if (_targetObject == null || string.IsNullOrEmpty(_targetMethodName) || string.IsNullOrEmpty(_targetMethodAsmName))
                return;
            var tgtType = _targetObject.GetType();
            //Make Sure the object is from the same Assembly as defined in Editor
            if (tgtType.AssemblyQualifiedName == _targetMethodAsmName)
            {
                _assemblyInfo = tgtType.Assembly;
                var methods = tgtType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
                foreach (var method in methods)
                {
                    if (method.Name == _targetMethodName && method.GetParameters().Length == 0)
                    {
                        _method = method;
                        _isValid = true;
                        break;
                    }
                }
            }
            else {
                Debug.LogError("This object '" + tgtType + "' has different assembly reference!");
            }
        }

        public virtual void Invoke() {
            if (_isValid) {
                _method.Invoke(_targetObject, null);
            }
            else {
                Validate();
                if (_isValid)
                    _method.Invoke(_targetObject, null);
                else
                    Debug.LogError("Failed to Find Method '" + _method + "' in object '" + _targetObject + "'");
            }
        }
    }

    [System.Serializable]
    public class ImoetComponentMethod<T> : ImoetComponentMethod {
        public ImoetComponentMethod(){
            _paramAsmName = typeof(T).AssemblyQualifiedName;
        }
        public override void Validate() {
            _isValid = false;
            _method = null;
            _assemblyInfo = null;

            //Ignore it if the field is empty / null
            if (_targetObject == null || string.IsNullOrEmpty(_targetMethodName) || string.IsNullOrEmpty(_targetMethodAsmName))
                return;

            var tgtParamType = typeof(T);
            var tgtType = _targetObject.GetType();

            //Make Sure the object is from the same Assembly as defined in Editor
            if (tgtType.Assembly.FullName == _targetMethodAsmName)
            {
                _assemblyInfo = tgtType.Assembly;
                var methods = tgtType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.InvokeMethod);
                foreach (var method in methods)
                {
                    var methodParams = method.GetParameters();
                    if (method.Name == _targetMethodName && methodParams.Length == 1) {
                        var methodParamsType = methodParams[0].GetType();
                        if (UnityExUtility.isUnityReadable(methodParamsType) || methodParamsType.IsAssignableFrom(tgtParamType)) {
                            _method = method;
                            _isValid = true;
                            return;
                        }
                    }
                }
            }
            else
                Debug.LogError("This object '" + tgtType + "' has different assembly reference!");
        }

        public override void Invoke() {
            Invoke(default);
        }

        public virtual void Invoke(T param) {
            if (_isValid)
                _method.Invoke(_targetObject, new object[] { param });
            else
            {
                Validate();
                if (_isValid)
                    _method.Invoke(_targetObject, new object[] { param });
                else
                    Debug.LogError("Failed to Find Method '" + _method + "' in object '" + _targetObject + "'");
            }
        }
    }
}
