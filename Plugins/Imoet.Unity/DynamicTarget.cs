using System;
using System.Collections.Generic;
using UnityEngine;
using UObject = UnityEngine.Object;

namespace Imoet.Unity
{
    public enum DynamicTargetType {
        Method,
        Field
    }

    [Serializable]
    public class DynamicTarget<T> {
        public List<DynamicTargetItem<T>> list;
    }

    [Serializable]
    public class DynamicTargetItem<T> {
        [SerializeField]
        private UObject m_obj;
        [SerializeField]
        private string m_method;
        [SerializeField]
        private T m_targetType;
        [SerializeField]
        private DynamicTargetType m_type;

        public UObject obj { get { return m_obj; } set { m_obj = value; } }
        public string method { get { return m_method; } set { m_method = value; } }
        public T paramType { get { return m_targetType; } set { m_targetType = value; } }
        public DynamicTargetType targetType { get { return m_type; } set { m_type = value; } }
    }
}