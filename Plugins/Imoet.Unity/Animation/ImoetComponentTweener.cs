using Imoet.Unity.Utility;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Imoet.Unity.Animation
{
    [DisallowMultipleComponent]
    public class ImoetComponentTweener : MonoBehaviour
    {
        [SerializeField]
        private List<Item> m_items;

        [System.Serializable]
        internal class Item {
            //Property
            public TweenSetting setting { get { return m_setting; } set { m_setting = value; } }
            public MethodInfo methodInfo { get { return m_methodInfo; } set { m_methodInfo = value; } }
            
            //Serialized Field
            [SerializeField]
            private string m_methodName;
            [SerializeField]
            private Component m_component;
            [SerializeField]
            private UnityExUtility.UnityReadableType m_valueType;
            [SerializeField]
            private TweenSetting m_setting;

            //Field
            private MethodInfo m_methodInfo;
            private readonly System.Object m_tweenerObj;

            //Value
            [SerializeField]
            UnityExInternalUtilty.tweenByte val_byte;
            [SerializeField]
            UnityExInternalUtilty.tweenShort val_short;
            [SerializeField]
            UnityExInternalUtilty.tweenInt val_int;
            [SerializeField]
            UnityExInternalUtilty.tweenFloat val_float;
            [SerializeField]
            UnityExInternalUtilty.tweenDouble val_double;
            [SerializeField]
            UnityExInternalUtilty.tweenVector2 val_vector2;
            [SerializeField]
            UnityExInternalUtilty.tweenVector3 val_vector3;
            [SerializeField]
            UnityExInternalUtilty.tweenVector4 val_vector4;
            [SerializeField]
            UnityExInternalUtilty.tweenQuaternion val_quaternion;
            [SerializeField]
            UnityExInternalUtilty.tweenRect val_rect;
            [SerializeField]
            UnityExInternalUtilty.tweenColor val_color;
            [SerializeField]
            UnityExInternalUtilty.tweenColor32 val_color32;
        }
    }
}

