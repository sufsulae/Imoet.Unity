using Imoet.Unity.Utility;
using System;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Imoet.Unity
{
    [Serializable]
    public class UnityMethod {
        [SerializeField,ReadOnly]
        private string m_methodName;
        [SerializeField]
        private UObject m_obj;
        [SerializeField]
        private UnityExUtility.UnityReadableType m_valueType;

        public string methodName { get { return m_methodName; } }
        public UObject obj { get { return m_obj; } }
        public UnityExUtility.UnityReadableType valueType { get { return m_valueType; } }
    }
}