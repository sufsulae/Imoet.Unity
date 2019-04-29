namespace Imoet.Unity.Animation
{
    [System.Serializable]
    public struct TweenSetting
    {
        public float duration, progress;
        public TweenMode mode;
        public TweenType type;
        public TweenDirection direction;
        public bool resetValue, forceTween;
    }
}
