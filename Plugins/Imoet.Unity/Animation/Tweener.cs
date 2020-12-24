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

        /// <summary>
        /// Delegate to process / apply calculated tween,must be assigned to make it work
        /// </summary>
        public Action<T> tweenDelegate { get; set; }
        /// <summary>
        /// Delegate to determine the calculation of tween
        /// </summary>
        public TweenCalculateValue<T> tweenCalcValue { get; set; }
        /// <summary>
        /// Setting for the tween
        /// </summary>
        public TweenSetting setting { get; set; } = default(TweenSetting);
        /// <summary>
        /// Status of Tween
        /// </summary>
        public TweenStatus status { get; private set; } = TweenStatus.Stop;
        /// <summary>
        /// Start Value to tween
        /// </summary>
        public T valueStart { get; set; }
        /// <summary>
        /// End Value to tween
        /// </summary>
        public T valueEnd { get; set; }
        /// <summary>
        /// Progress
        /// </summary>
        public float progress {
            get { return setting.progress; }
            set {
                var set = setting;
                set.progress = value;
                setting = set;
            }
        }
        /// <summary>
        /// Delegate that will executed when this tween is working
        /// </summary>
        public Action onTweening { get; set; }

        /// <summary>
        /// Function to Starting the tween
        /// </summary>
        public virtual void StartTween()
        {
            if (valueStart.Equals(valueEnd)) {
                Debug.Log("ValueStart and ValueEnd are Equal, Ignoring!");
                return;
            }
            var t_set = setting;
            if (!t_set.forceTween && status == TweenStatus.Tween)
            {
                Debug.Log("Tweener is Tweening!");
                return;
            }
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
            if (valueStart.Equals(valueEnd))
                return;
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
            if (status != TweenStatus.Tween)
                return;
            var tempSetting = setting;
            //Get Last Status and stop if we already reach the end
            if (status != TweenStatus.Tween && setting.duration == 0) {
                status = TweenStatus.Stop;
                return;
            }
            //Try to fix in case direction is Zero
            if (tempSetting.direction == 0) {
                tempSetting.direction = TweenDirection.Forward;
            }
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
						if(onTweening != null)
							onTweening.Invoke();
                    }
                    break;
                case TweenMode.Loop:
                    if (tempSetting.progress > 1 || tempSetting.progress < 0)
                    {
                        tempSetting.progress = 0;
                        if(onTweening != null)
							onTweening.Invoke();
                    }
                    break;
                case TweenMode.Once:
                    if (tempSetting.progress > 1 || tempSetting.progress < 0)
                    {
                        tempSetting.progress = Mathf.Clamp01(tempSetting.progress);
                        status = TweenStatus.Stop;
						if(onTweening != null)
							onTweening.Invoke();
                    }
                    break;
            }
            //Apply Tween
            if (tweenCalcValue != null && tweenDelegate != null) {
                tweenDelegate.Invoke(tweenCalcValue.Invoke(Tween.Evaluate(0.0f, 1.0f, tempSetting.progress, tempSetting.type)));
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

        //Helper Function
        public static Tweener<float> CreateTweenFloat(float start, float end) {
            var tween = new Tweener<float>();
            tween.valueStart = start;
            tween.valueEnd = end;
            tween.tweenCalcValue = (percentage) => {
                return start + (end - start) * percentage;
            };
            return tween;
        }
        public static Tweener<double> CreateTweenDouble(double start, double end) {
            var tween = new Tweener<double>();
            tween.valueStart = start;
            tween.valueEnd = end;
            tween.tweenCalcValue = (percentage) => {
                return start + (end - start) * percentage;
            };
            return tween;
        }
        public static Tweener<int> CreateTweenInt(int start, int end)
        {
            var tween = new Tweener<int>();
            tween.valueStart = start;
            tween.valueEnd = end;
            tween.tweenCalcValue = (percentage) => {
                return (int)(start + (end - start) * percentage);
            };
            return tween;
        }
        public static Tweener<Vector2> CreateTweenVector2(Vector2 start, Vector2 end) {
            var tween = new TweenVector2();
            tween.valueStart = start;
            tween.valueEnd = end;
            return tween;
        }
        public static Tweener<Vector3> CreateTweenVector3(Vector3 start, Vector3 end)
        {
            var tween = new TweenVector3();
            tween.valueStart = start;
            tween.valueEnd = end;
            return tween;
        }
        public static Tweener<Vector4> CreateTweenVector4(Vector4 start, Vector4 end)
        {
            var tween = new TweenVector4();
            tween.valueStart = start;
            tween.valueEnd = end;
            return tween;
        }
        public static Tweener<Quaternion> CreateTweenQuaternion(Quaternion start, Quaternion end)
        {
            var tween = new TweenQuaternion();
            tween.valueStart = start;
            tween.valueEnd = end;
            return tween;
        }
        public static Tweener<Rect> CreateTweenRect(Rect start, Rect end)
        {
            var tween = new TweenRect();
            tween.valueStart = start;
            tween.valueEnd = end;
            return tween;
        }
        public static Tweener<Color> CreateTweenColor(Color start, Color end)
        {
            var tween = new TweenColor();
            tween.valueStart = start;
            tween.valueEnd = end;
            return tween;
        }
        public static Tweener<Color32> CreateTweenColor32(Color32 start, Color32 end)
        {
            var tween = new TweenColor32();
            tween.valueStart = start;
            tween.valueEnd = end;
            return tween;
        }
    }
}
