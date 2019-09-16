namespace Imoet.Unity.Animation
{
    /// <summary>
    /// Interface for <see cref="Tweener{T}"/>
    /// </summary>
    public interface ITweener
    {
        TweenSetting setting { get; set; }
        TweenStatus status { get; }

        void StartTween();
        void StopTween();
        void PauseTween();
        void StartReverseTween();
        void SwapAndStart();
        void Update(float updateTime);
        void Update();
    }
}
