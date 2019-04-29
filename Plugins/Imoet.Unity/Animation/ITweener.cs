namespace Imoet.Unity.Animation
{
    public interface ITweener
    {
        void StartTween();
        void StopTween();
        void PauseTween();
        void StartReverseTween();
        void SwapAndStart();
        void Update(float updateTime);
    }
}
