using UnityEngine;

namespace Imoet.Unity.Animation
{
    [System.Serializable]
    public class TweenVector2 : Tweener<Vector2>
    {
        public TweenVector2() {
            tweenCalc = (p) => { return valueStart + (valueEnd - valueStart) * p; };
        }
    }
    [System.Serializable]
    public class TweenVector3 : Tweener<Vector3>
    {
        public TweenVector3() {
            tweenCalc = (p) => { return valueStart + (valueEnd - valueStart) * p; };
        }
    }
    [System.Serializable]
    public class TweenVector4 : Tweener<Vector4>
    {
        public TweenVector4() {
            tweenCalc = (p) => { return valueStart + (valueEnd - valueStart) * p; };
        }
    }
}
