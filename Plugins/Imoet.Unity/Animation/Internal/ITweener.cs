namespace Imoet.Unity.Animation {
    public interface ITweener
    {
        void StartTween();
        void StartTweenReverse();
        void PauseTween();
        void StopTween();
        void Update(float time);
        void Evaluate(float progress);
    }
}