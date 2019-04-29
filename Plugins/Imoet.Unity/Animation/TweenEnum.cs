namespace Imoet.Unity.Animation
{
    public enum TweenType
    {
        Linear,
        ContinuousLinear,
        Spring,
        Punch,
        EaseInExpo,
        EaseOutExpo,
        EaseInOutExpo,
        EaseInCircular,
        EaseOutCircular,
        EaseInOutCircular,
        EaseInQuadratic,
        EaseOutQuadratic,
        EaseInOutQuadratic,
        EaseInCubic,
        EaseOutCubic,
        EaseInOutCubic,
        EaseInQuartic,
        EaseOutQuartic,
        EaseInOutQuartic,
        EaseInQuintic,
        EaseOutQuintic,
        EaseInOutQuintic,
        EaseInSinusiodal,
        EaseOutSinusiodal,
        EaseInOutSinusiodal,
        EaseInBounce,
        EaseOutBounce,
        EaseInOutBounce,
        EaseInBack,
        EaseOutBack,
        EaseInOutBack,
        EaseInElastic,
        EaseOutElastic,
        EaseInOutElastic,
    }

    public enum TweenMode
    {
        Loop,
        PingPong,
        Once
    }
    public enum TweenStatus
    {
        Tween,
        Pause,
        Stop
    }
    public enum TweenDirection
    {
        Forward = 1,
        Reverse = -1
    }
}
