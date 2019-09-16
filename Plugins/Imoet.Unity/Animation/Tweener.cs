using UnityEngine;

namespace Imoet.Unity.Animation
{
    using System;
    /// <summary>
    /// Class to make interpolation between 2 values easier.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Tweener<T> : ITweener
    {
        //Compatibility for reflection
        private TweenSetting m_setting;
        private TweenStatus m_status;
        private T m_valStart, m_valEnd;

        /// <summary>
        /// Delegate to process / apply calculated tween,must be assigned to make it work
        /// </summary>
        public Action<T> tweenDelegate { get; set; }
        /// <summary>
        /// Delegate to determine the calculation of tween, must be assigned to make it work
        /// </summary>
        public Func<float, T> tweenCalc { get; set; }
        /// <summary>
        /// Setting for the tween
        /// </summary>
        public TweenSetting setting { get { return m_setting; } set { m_setting = value; } }
        /// <summary>
        /// Status of Tween
        /// </summary>
        public TweenStatus status { get { return m_status; } private set { m_status = value; } }
        /// <summary>
        /// Start Value to tween
        /// </summary>
        public T valueStart { get { return m_valStart; } set { m_valStart = value; } }
        /// <summary>
        /// End Value to tween
        /// </summary>
        public T valueEnd { get { return m_valEnd; } set { m_valEnd = value; } }
        /// <summary>
        /// Delegate that will executed when this tween is working
        /// </summary>
        public Action onTweenEvent { get; set; }

        /// <summary>
        /// Function to Starting the tween
        /// </summary>
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
        /// <summary>
        /// Function to Stoping the tween
        /// </summary>
        public virtual void StopTween()
        {
            var t_set = setting;
            t_set.progress = 0;
            t_set.direction = 0;
            status = TweenStatus.Stop;
            setting = t_set;
        }
        /// <summary>
        /// Function to Pause the Tween
        /// </summary>
        public virtual void PauseTween()
        {
            status = TweenStatus.Pause;
        }
        /// <summary>
        /// Function to Start the tween in reverse mode
        /// </summary>
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
        /// <summary>
        /// Function to Swap Between <see cref="valueStart"/> and <see cref="valueEnd"/>
        /// </summary>
        public virtual void SwapValue()
        {
            var _t = valueStart;
            valueStart = valueEnd;
            valueEnd = _t;
        }
        /// <summary>
        /// Function to Swap Between <see cref="valueStart"/> and <see cref="valueEnd"/> then immidietly started the tween
        /// </summary>
        public virtual void SwapAndStart()
        {
            SwapValue();
            StartTween();
        }
        /// <summary>
        /// Function to Update the tween, must be executed into continous routines / loop
        /// </summary>
        /// <param name="updateTime"></param>
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
            if (tweenCalc != null && tweenDelegate != null) {
                tweenDelegate.Invoke(tweenCalc.Invoke(Tween.Evaluate(0.0f, 1.0f, tempSetting.progress, tempSetting.type)));
            }
            //Apply Updated Setting
            setting = tempSetting;
        }
        /// <summary>
        /// Function to Update the tween designated for Unity
        /// </summary>
        public virtual void Update() {
            Update(Time.deltaTime);
        }
    }
}
