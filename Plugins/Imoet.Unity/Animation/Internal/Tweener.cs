using System;
using UnityEngine;

namespace Imoet.Unity.Animation
{
    [Serializable]
    public abstract class Tweener : ITweener
    {
        //Variables
        public virtual float progress { get; set; }
        public virtual TweenSetting settings { get; set; }
        public virtual TweenStatus status { get; set; }
        public virtual TweenMode mode
        {
            get { return settings.mode; }
            set
            {
                var setting = settings;
                setting.mode = value;
                settings = setting;
            }
        }
        public virtual TweenType type
        {
            get { return settings.type; }
            set
            {
                var setting = settings;
                setting.type = value;
                settings = setting;
            }
        }
        public virtual TweenDirection direction
        {
            get { return settings.direction; }
            set
            {
                var setting = settings;
                setting.direction = value;
                settings = setting;
            }
        }
        public virtual float duration
        {
            get { return settings.duration; }
            set
            {
                var setting = settings;
                setting.duration = value;
                settings = setting;
            }
        }

        //Event
        public virtual Action OnTweenStart { get; set; }
        public virtual Action OnTweenEnd { get; set; }

        public virtual void PauseTween() { }
        public virtual void StartTween() { }
        public virtual void StartTweenReverse() { }
        public virtual void StopTween() { }
        public virtual void Update(float time) { }
        public virtual void Evaluate(float progress) { }
    }

    [Serializable]
    public class Tweener<T> : Tweener {
        public T valueFrom { get; set; }
        public T valueTo { get; set; }
        public TweenUpdateDelegate<T> OnTweenUpdate { get; set; }
        public TweenCalculateDelegate<T> OnTweenCalculate { get; set; }

        public override TweenSetting settings {
            get { return _setting; }
            set {
                _setting = value;
                _settingUpdated = true;
            }
        }
        public override float progress
        {
            get { return _progressTime / settings.duration; }
            set { base.progress = value; }
        }

        private bool _pong;
        private bool _settingUpdated;
        private TweenSetting _setting;
        private float _progressTime;

        public void Update()
        {
            Update(Time.deltaTime);
        }

        public override void Update(float time) {
            //Exception
            if (_settingUpdated) {
                if (settings.duration <= 0) {
#if UNITY_EDITOR
                    Debug.Log("Tweener: Duration is Zero, Ignoring!");
#endif
                    return;
                }
                if (settings.direction == TweenDirection.None) {
#if UNITY_EDITOR
                    Debug.Log("Tweener: Direction is None, Ignoring!");
#endif
                    return;
                }
                OnTweenStart?.Invoke();
                _settingUpdated = false;
            }

            //Calculation
            if (status == TweenStatus.Tween && OnTweenCalculate != null) {
                _progressTime += time * (int)settings.direction * (_pong ? -1 : 1);
                if (_progressTime > settings.duration || _progressTime < 0 ) {
                    switch (settings.mode) {
                        case TweenMode.Once:
                            status = TweenStatus.Stop;
                            break;
                        case TweenMode.Loop:
                            if (settings.direction == TweenDirection.Forward)
                                _progressTime = 0;
                            else if (settings.direction == TweenDirection.Reverse)
                                _progressTime = settings.duration;
                            break;
                        case TweenMode.PingPong:
                            _pong = !_pong;
                            break;
                    }
                    OnTweenEnd?.Invoke();
                    _progressTime = Mathf.Clamp(progress, 0.0f, settings.duration);
                }
                progress = _progressTime / settings.duration;
                Evaluate(progress);
            }
        }

        public override void Evaluate(float progress) {
            OnTweenUpdate?.Invoke(OnTweenCalculate.Invoke(Tween.Evaluate(0.0f, 1.0f, progress, settings.type)));
        }

        public override void StartTween() {
            if(status != TweenStatus.Tween)
                status = TweenStatus.Tween;
        }

        public override void StartTweenReverse()
        {
            if (status != TweenStatus.Tween) {
                var set = settings;
                if (status == TweenStatus.Stop)
                {
                    if (set.direction == TweenDirection.Forward)
                        _progressTime = set.duration;
                    else if (set.direction == TweenDirection.Reverse)
                        _progressTime = 0;
                }
                else if (status == TweenStatus.Pause)
                {
                    if (settings.direction == TweenDirection.Forward)
                        set.direction = TweenDirection.Reverse;
                    else if (settings.direction == TweenDirection.Reverse)
                        set.direction = TweenDirection.Forward;
                    settings = set;

                }
                StartTween();
            }
        }

        public override void PauseTween()
        {
            if (status != TweenStatus.Pause)
                status = TweenStatus.Pause;
        }

        public override void StopTween()
        {
            if (status != TweenStatus.Stop) {
                status = TweenStatus.Stop;
                _progressTime = 0;
                _pong = false;
            }
        }
    }
}
