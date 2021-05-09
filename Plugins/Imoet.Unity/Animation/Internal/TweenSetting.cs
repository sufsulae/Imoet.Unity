using System;

namespace Imoet.Unity.Animation
{
    [Serializable]
    public struct TweenSetting {
        public TweenMode mode;
        public TweenType type;
        public TweenDirection direction;
        //public bool resetValue, forceTween;
        public float duration, startProgress;
    }
}