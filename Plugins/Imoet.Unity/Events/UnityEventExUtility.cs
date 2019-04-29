using System;

namespace Imoet.Unity.Events
{
    public static class UnityEventExUtility
    {
        public static Type[] UnityReadableTypeList = new Type[]{
            typeof(void),
            typeof(byte),
            typeof(short),
            typeof(int),
            typeof(float),
            typeof(double),
            typeof(bool),
            typeof(string),
            typeof(UnityEngine.Vector2),
            typeof(UnityEngine.Vector3),
            typeof(UnityEngine.Vector4),
            typeof(UnityEngine.Quaternion),
            typeof(UnityEngine.Color),
            typeof(UnityEngine.Color32),
            typeof(UnityEngine.Rect),
            typeof(UnityEngine.Object),
            typeof(Enum)
        };
        [System.Serializable]
        public class UnityReadableValue
        {
            public string m_string;
            public bool m_bool;
            public byte m_byte;
            public short m_short;
            public int m_int;
            public float m_float;
            public double m_double;
            public int m_enum;
            public UnityEngine.Vector2 m_vector2;
            public UnityEngine.Vector3 m_vector3;
            public UnityEngine.Vector4 m_vector4;
            public UnityEngine.Quaternion m_quaternion;
            public UnityEngine.Color32 m_color32;
            public UnityEngine.Color m_color;
            public UnityEngine.Rect m_rect;
            public UnityEngine.Object m_unityObject;
        }

        public enum UnityReadableType
        {
            None = -1,
            Void,
            Byte,
            Short,
            Int,
            Float,
            Double,
            Boolean,
            String,
            Vector2,
            Vector3,
            Vector4,
            Quaternion,
            Color,
            Color32,
            Rect,
            UnityObject,
            Enum
        }
        public static bool isUnityReadable(Type type)
        {
            if (type.IsEnum)
                return true;
            foreach (Type t in UnityReadableTypeList)
            {
                if (t.IsAssignableFrom(type) || t == type)
                    return true;
            }
            return false;
        }
        public static Type getUnityReadableEquivalent(Type type)
        {
            if (type.IsEnum)
                return typeof(Enum);
            foreach (Type t in UnityReadableTypeList)
            {
                if (t.IsAssignableFrom(type) || t == type)
                {
                    return t;
                }
            }
            return null;
        }
        public static UnityReadableType getUnityReadableType(Type type)
        {
            if (type.IsEnum)
                return UnityReadableType.Enum;
            int length = UnityReadableTypeList.Length;
            for (int i = 0; i < length; i++)
            {
                if (UnityReadableTypeList[i].IsAssignableFrom(type) || UnityReadableTypeList[i] == type)
                    return (UnityReadableType)(i);
            }
            return UnityReadableType.None;
        }
    }
}
