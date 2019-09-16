using UnityEngine;
using System;

namespace Imoet.Unity.Animation
{
    [Serializable]
    public class TweenVector2 : Tweener<Vector2>
    {
        public TweenVector2() {
            tweenCalc = (p) => { return valueStart + (valueEnd - valueStart) * p; };
        }
    }
    [Serializable]
    public class TweenVector3 : Tweener<Vector3>
    {
        public TweenVector3() {
            tweenCalc = (p) => { return valueStart + (valueEnd - valueStart) * p; };
        }
    }
    [Serializable]
    public class TweenVector4 : Tweener<Vector4>
    {
        public TweenVector4() {
            tweenCalc = (p) => { return valueStart + (valueEnd - valueStart) * p; };
        }
    }
    [Serializable]
    public class TweenQuaternion : Tweener<Quaternion> {
        public TweenQuaternion() {
            tweenCalc = (p) => { return Quaternion.Lerp(valueStart, valueEnd, p); };
        }
    }
    [Serializable]
    public class TweenRect : Tweener<Rect> {
        public TweenRect() {
            tweenCalc = (p) => {
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
            tweenCalc = (p) => {
                return Color.Lerp(valueStart, valueEnd, p);
            };
        }
    }
    [Serializable]
    public class TweenColor32 : Tweener<Color32> {
        public TweenColor32() {
            tweenCalc = (p) => {
                return Color32.Lerp(valueStart, valueEnd, p);
            };
        }
    }
}
