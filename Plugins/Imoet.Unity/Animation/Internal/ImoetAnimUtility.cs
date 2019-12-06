using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Imoet.Unity.Animation {
    internal interface ITweenVal
    {
        void Initialize();
        ITweener GetTweener();
    }
    internal class ImoetAnimUtility
    {
        [Serializable]
        internal class tweenByte : TweenerVal<byte> { }
        [Serializable]
        internal class tweenShort : TweenerVal<short> { }
        [Serializable]
        internal class tweenInt : TweenerVal<int> { }
        [Serializable]
        internal class tweenFloat : TweenerVal<float> { }
        [Serializable]
        internal class tweenDouble : TweenerVal<double> { }
        [Serializable]
        internal class tweenVector2 : TweenerVal<Vector2> { }
        [Serializable]
        internal class tweenVector3 : TweenerVal<Vector3> { }
        [Serializable]
        internal class tweenVector4 : TweenerVal<Vector4> { }
        [Serializable]
        internal class tweenQuaternion : TweenerVal<Quaternion> { }
        [Serializable]
        internal class tweenRect : TweenerVal<Rect> { }
        [Serializable]
        internal class tweenColor : TweenerVal<Color> { }
        [Serializable]
        internal class tweenColor32 : TweenerVal<Color32> { }

        [Serializable]
        internal class TweenerVal<T> : ITweenVal
        {
            public Tweener<T> tweener;
            public T valStart, valEnd;
            public TweenSetting tweenerSetting;

            public void Initialize()
            {
                tweener = new Tweener<T>();
                tweener.setting = tweenerSetting;
                tweener.valueStart = valStart;
                tweener.valueEnd = valEnd;
            }

            public ITweener GetTweener() {
                return tweener;
            }
        }
    }
}