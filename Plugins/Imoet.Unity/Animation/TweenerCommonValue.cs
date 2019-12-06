using UnityEngine;
using System;

namespace Imoet.Unity.Animation
{
    [Serializable]
    public class TweenVector2 : Tweener<Vector2>
    {
        public TweenVector2() {
            tweenCalcValue = (p) => { return valueStart + (valueEnd - valueStart) * p; };
        }
    }
    [Serializable]
    public class TweenVector3 : Tweener<Vector3>
    {
        public TweenVector3() {
            tweenCalcValue = (p) => { return valueStart + (valueEnd - valueStart) * p; };
        }
    }
    [Serializable]
    public class TweenVector4 : Tweener<Vector4>
    {
        public TweenVector4() {
            tweenCalcValue = (p) => { return valueStart + (valueEnd - valueStart) * p; };
        }
    }
    [Serializable]
    public class TweenQuaternion : Tweener<Quaternion> {
        public TweenQuaternion() {
            tweenCalcValue = (p) => { return Quaternion.Lerp(valueStart, valueEnd, p); };
        }
    }
    [Serializable]
    public class TweenRect : Tweener<Rect> {
        public TweenRect() {
            tweenCalcValue = (p) => {
                var rect = default(Rect);
                rect.x = Mathf.Lerp(valueStart.x, valueEnd.x, p);
                rect.y = Mathf.Lerp(valueStart.y, valueEnd.y, p);
                rect.width = Mathf.Lerp(valueStart.width, valueEnd.width, p);
                rect.height = Mathf.Lerp(valueStart.height, valueEnd.height, p);
                return rect;
            };
        }
    }
    [Serializable]
    public class TweenColor : Tweener<Color> {
        public TweenColor() {
            tweenCalcValue = (p) => {
                return Color.Lerp(valueStart, valueEnd, p);
            };
        }
    }
    [Serializable]
    public class TweenColor32 : Tweener<Color32> {
        public TweenColor32() {
            tweenCalcValue = (p) => {
                return Color32.Lerp(valueStart, valueEnd, p);
            };
        }
    }
}
