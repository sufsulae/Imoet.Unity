using UnityEngine;

namespace Imoet.Unity.Animation
{
    using System;
    [Serializable]
    public class Tweener<T> : ITweener
    {
        //Compatibility for reflection
        private TweenSetting m_setting;
        private TweenStatus m_status;
        private T m_valStart, m_valEnd;

        public Action<T> tweenDelegate { get; set; }
        public Func<float, T> tweenCalc { get; set; }
        public TweenSetting setting { get { return m_setting; } set { m_setting = value; } }
        public TweenStatus status { get { return m_status; } private set { m_status = value; } }
        public T valueStart { get { return m_valStart; } set { m_valStart = value; } }
        public T valueEnd { get { return m_valEnd; } set { m_valEnd = value; } }
        public Action onTweenEvent { get; set; }

        public virtual void StartTween()
        {
            var t_set = setting;
            if (!t_set.forceTween && status == TweenStatus.Tween)
                return;
            status = TweenStatus.Tween;
            if (status != TweenStatus.Pause)
            {
                if (t_set.resetValue)
                    t_set.progress = 0;
                else if (t_set.progress == 1)
                    t_set.progress = 0;
                t_set.direction = TweenDirection.Forward;
            }
            setting = t_set;
        }
        public virtual void StopTween()
        {
            var t_set = setting;
            t_set.progress = 0;
            t_set.direction = 0;
            status = TweenStatus.Stop;
            setting = t_set;
        }
        public virtual void PauseTween()
        {
            status = TweenStatus.Pause;
        }
        public virtual void StartReverseTween()
        {
            var t_set = setting;
            if (!t_set.forceTween && status == TweenStatus.Tween)
                return;
            if (status != TweenStatus.Pause)
            {
                if (t_set.resetValue)
                    t_set.progress = 1;
                else if (t_set.progress == 0)
                    t_set.progress = 1;
                t_set.direction = TweenDirection.Reverse;
            }
            status = TweenStatus.Tween;
            setting = t_set;
        }
        public virtual void SwapValue()
        {
            var _t = valueStart;
            valueStart = valueEnd;
            valueEnd = _t;
        }
        public virtual void SwapAndStart()
        {
            SwapValue();
            StartTween();
        }

        public virtual void Update(float updateTime)
        {
            var tempSetting = setting;
            //Get Last Status and stop if we already reach the end
            if (status != TweenStatus.Tween || setting.duration == 0)
                return;
            //Calculate Progress by updateTime
            tempSetting.progress += updateTime / tempSetting.duration * (int)setting.direction;
            switch (setting.mode)
            {
                case TweenMode.PingPong:
                    if (tempSetting.progress > 1 || tempSetting.progress < 0)
                    {
                        tempSetting.progress = Mathf.Clamp01(tempSetting.progress);
                        if (setting.direction == TweenDirection.Forward)
                            tempSetting.direction = TweenDirection.Reverse;
                        else
                            tempSetting.direction = TweenDirection.Forward;
						if(onTweenEvent != null)
							onTweenEvent.Invoke();
                    }
                    break;
                case TweenMode.Loop:
                    if (tempSetting.progress > 1 || tempSetting.progress < 0)
                    {
                        tempSetting.progress = 0;
                        if(onTweenEvent != null)
							onTweenEvent.Invoke();
                    }
                    break;
                case TweenMode.Once:
                    if (tempSetting.progress > 1 || tempSetting.progress < 0)
                    {
                        tempSetting.progress = Mathf.Clamp01(tempSetting.progress);
                        status = TweenStatus.Stop;
						if(onTweenEvent != null)
							onTweenEvent.Invoke();
                    }
                    break;
            }
            //Apply Tween
            if (tweenCalc != null && tweenDelegate != null)
                tweenDelegate.Invoke(tweenCalc.Invoke(Tween.Evaluate(0.0f, 1.0f, tempSetting.progress, tempSetting.type)));
            //Apply Updated Setting
            setting = tempSetting;
        }
        public virtual void Update() {
            Update(Time.deltaTime);
        }
    }
}
