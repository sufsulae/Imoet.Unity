using System;
using UnityEngine;

namespace Imoet.Unity.Animation.Template {
    [Serializable]
    public class TweenByte : Tweener<byte>
    {
        public TweenByte()
        {
            OnTweenCalculate = (v) => {
                return (byte)(valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenShort : Tweener<short>
    {
        public TweenShort()
        {
            OnTweenCalculate = (v) => {
                return (short)(valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenInt : Tweener<int>
    {
        public TweenInt()
        {
            OnTweenCalculate = (v) => {
                return (int)(valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenLong : Tweener<long>
    {
        public TweenLong()
        {
            OnTweenCalculate = (v) => {
                return (int)(valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenFloat : Tweener<float>
    {
        public TweenFloat()
        {
            OnTweenCalculate = (v) => {
                return (float)(valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenDouble : Tweener<double>
    {
        public TweenDouble()
        {
            OnTweenCalculate = (v) => {
                return (double)(valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenVector2 : Tweener<Vector2>
    {
        public TweenVector2()
        {
            OnTweenCalculate = (v) => {
                return (valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenVector3 : Tweener<Vector3>
    {
        public TweenVector3()
        {
            OnTweenCalculate = (v) => {
                return (valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenVector4 : Tweener<Vector4>
    {
        public TweenVector4()
        {
            OnTweenCalculate = (v) => {
                return (valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenColor : Tweener<Color>
    {
        public TweenColor()
        {
            OnTweenCalculate = (v) => {
                return (valueFrom + (valueTo - valueFrom) * v);
            };
        }
    }

    [Serializable]
    public class TweenRect : Tweener<Rect>
    {
        public TweenRect()
        {
            OnTweenCalculate = (v) => {
                Rect r = new Rect();
                r.x = Mathf.Lerp(valueFrom.x, valueTo.x, v);
                r.y = Mathf.Lerp(valueFrom.y, valueTo.y, v);
                r.width = Mathf.Lerp(valueFrom.width, valueTo.width, v);
                r.height = Mathf.Lerp(valueFrom.height, valueTo.height, v);
                return r;
            };
        }
    }
}

namespace Imoet.Unity.Animation {
    using Template;
    public static class ImoetTween {
        //Template Creation
        public static TweenByte CreateTween(byte from, byte to, float duration) {
            return new TweenByte() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenShort CreateTween(short from, short to, float duration) {
            return new TweenShort() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenInt CreateTween(int from, int to, float duration) {
            return new TweenInt() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenLong CreateTween(long from, long to, float duration) {
            return new TweenLong() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenFloat CreateTween(float from, float to, float duration) {
            return new TweenFloat() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenDouble CreateTween(double from, double to, float duration) {
            return new TweenDouble() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenVector2 CreateTween(Vector2 from, Vector2 to, float duration) {
            return new TweenVector2() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenVector3 CreateTween(Vector3 from, Vector3 to, float duration) {
            return new TweenVector3() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenVector4 CreateTween(Vector4 from, Vector4 to, float duration) {
            return new TweenVector4() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenColor CreateTween(Color from, Color to, float duration) {
            return new TweenColor() { valueFrom = from, valueTo = to, duration = duration };
        }
        public static TweenRect CreateTween(Rect from, Rect to, float duration) {
            return new TweenRect() { valueFrom = from, valueTo = to, duration = duration };
        }

        //Extention
        public static void TweenPosition(this Transform t, Vector3 from, Vector3 to) {

        }
        public static void TweenPosition(this Transform t, Vector2 from, Vector2 to) {

        }
        public static void TweenRotation(this Transform t, Vector3 from, Vector3 to) { }
        public static void TweenScale(this Transform t, Vector3 from, Vector3 to) { }
        public static void TweenTransform(this Transform t, Transform target) { }

        public static void TweenPath(this Transform t, Curve curve) {

        }
    }
}