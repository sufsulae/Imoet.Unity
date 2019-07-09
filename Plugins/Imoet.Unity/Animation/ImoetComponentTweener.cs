using UnityEngine;
using Imoet.Unity.Events;
using System.Reflection;
using System.Collections.Generic;

namespace Imoet.Unity.Animation
{
    public class ImoetComponentTweener : MonoBehaviour
    {
        [SerializeField]
        private List<Item> m_items;
        internal static readonly System.Type[] __readedType;
        static ImoetComponentTweener() {
            __readedType = new System.Type[] {
                typeof(byte),
                typeof(short),
                typeof(int),
                typeof(float),
                typeof(double),
                typeof(Vector2),
                typeof(Vector3),
                typeof(Vector4),
                typeof(Quaternion),
                typeof(Rect),
                typeof(Color),
                typeof(Color32)
            };
        }

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
            private UnityEventExUtility.UnityReadableType m_valueType;
            [SerializeField]
            private TweenSetting m_setting;

            //Field
            private MethodInfo m_methodInfo;
            private readonly System.Object m_tweenerObj;

            //Value
            TweenerVal<byte> val_byte;
            TweenerVal<short> val_short;
            TweenerVal<int> val_int;
            TweenerVal<float> val_float;
            TweenerVal<double> val_double;
            TweenerVal<Vector2> val_vector2;
            TweenerVal<Vector3> val_vector3;
            TweenerVal<Vector4> val_vector4;
            TweenerVal<Quaternion> val_quaternion;
            TweenerVal<Rect> val_rect;
            TweenerVal<Color> val_color;
            TweenerVal<Color32> val_color32;
        }
        [System.Serializable]
        internal class TweenerVal<T> {
            public Tweener<T> tweener;
            public T valIn, valOut;
            public TweenSetting tweenerSetting;
        }
    }
}

