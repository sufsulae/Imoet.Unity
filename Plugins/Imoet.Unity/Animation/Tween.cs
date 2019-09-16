using UnityEngine;
namespace Imoet.Unity.Animation
{
    /// <summary>
    /// Tween / Easing equation based on Robert Penner Equation:
    /// http://robertpenner.com/easing/
    /// </summary>
    public static class Tween
    {
        private const float s1Const = 1.70158f;
        private const float s2Const = 2.594909f;
        private const float Epsilon = 1E-05f;
        private const float maxAngle = 360.0f;
        private const float halfMaxAngle = 180.0f;
        private const float halfPI = 1.57079637f;
        private const float doublePI = 6.28318548f;
        private const float PI = 3.14159274f;
        public static float Evaluate(float start, float end, float time, TweenType type)
        {
            if (Mathf.Abs(start - end) < float.Epsilon)
                return start;
            switch (type)
            {
                case TweenType.Linear:
                    return start + (end - start) * time;
                case TweenType.ContinuousLinear:
                    var difEndStart = (end - start);
                    if ((difEndStart) < -halfMaxAngle)
                        return start + ((maxAngle - start) + end) * time;
                    if (difEndStart > halfMaxAngle)
                        return start - ((maxAngle - end) + start) * time;
                    return start + difEndStart * time;
                case TweenType.Spring:
                    time = Mathf.Clamp01(time);
                    time = (Mathf.Sin(time * Mathf.PI * (0.2f + 2.5f * time * time * time)) * Mathf.Pow(1f - time, 2.2f + time)) *
                           (1f + (1.2f * (1f - time)));
                    return start + (end - start) * time;
                case TweenType.EaseInQuadratic:
                    end -= start;
                    return end * time * time + start;
                case TweenType.EaseOutQuadratic:
                    end -= start;
                    return -end * time * (time - 2) + start;
                case TweenType.EaseInOutQuadratic:
                    time /= .5f;
                    end -= start;
                    if (time < 1) return end * 0.5f * time * time + start;
                    time--;
                    return -end * 0.5f * (time * (time - 2) - 1) + start;
                case TweenType.EaseInCubic:
                    end -= start;
                    return end * time * time * time + start;
                case TweenType.EaseOutCubic:
                    time--;
                    end -= start;
                    return end * (time * time * time + 1) + start;
                case TweenType.EaseInOutCubic:
                    time /= .5f;
                    end -= start;
                    if (time < 1)
                        return end * 0.5f * time * time * time + start;
                    time -= 2;
                    return end * 0.5f * (time * time * time + 2) + start;
                case TweenType.EaseInQuartic:
                    end -= start;
                    return end * time * time * time * time + start;
                case TweenType.EaseOutQuartic:
                    time--;
                    end -= start;
                    return -end * (time * time * time * time - 1) + start;
                case TweenType.EaseInOutQuartic:
                    time /= .5f;
                    end -= start;
                    if (time < 1)
                        return end * 0.5f * time * time * time * time + start;
                    time -= 2;
                    return -end * 0.5f * (time * time * time * time - 2) + start;
                case TweenType.EaseInQuintic:
                    end -= start;
                    return end * time * time * time * time * time + start;
                case TweenType.EaseOutQuintic:
                    time--;
                    end -= start;
                    return end * (time * time * time * time * time + 1) + start;
                case TweenType.EaseInOutQuintic:
                    time /= .5f;
                    end -= start;
                    if (time < 1)
                        return end * 0.5f * time * time * time * time * time + start;
                    time -= 2;
                    return end * 0.5f * (time * time * time * time * time + 2) + start;
                case TweenType.EaseInSinusiodal:
                    end -= start;
                    return -end * Mathf.Cos(time * halfPI) + end + start;
                case TweenType.EaseOutSinusiodal:
                    end -= start;
                    return end * Mathf.Sin(time * halfPI) + start;
                case TweenType.EaseInOutSinusiodal:
                    end -= start;
                    return -end * 0.5f * (Mathf.Cos(Mathf.PI * time) - 1) + start;
                case TweenType.EaseInExpo:
                    end -= start;
                    return end * Mathf.Pow(2, 10 * (time - 1)) + start;
                case TweenType.EaseOutExpo:
                    end -= start;
                    return end * (-Mathf.Pow(2, -10 * time) + 1) + start;
                case TweenType.EaseInOutExpo:
                    time /= .5f;
                    end -= start;
                    if (time < 1)
                        return end * 0.5f * Mathf.Pow(2, 10 * (time - 1)) + start;
                    time--;
                    return end * 0.5f * (-Mathf.Pow(2, -10 * time) + 2) + start;
                case TweenType.EaseInCircular:
                    end -= start;
                    return -end * (Mathf.Sqrt(1 - time * time) - 1) + start;
                case TweenType.EaseOutCircular:
                    time--;
                    end -= start;
                    return end * Mathf.Sqrt(1 - time * time) + start;
                case TweenType.EaseInOutCircular:
                    time /= .5f;
                    end -= start;
                    if (time < 1) return -end * 0.5f * (Mathf.Sqrt(1 - time * time) - 1) + start;
                    time -= 2;
                    return end * 0.5f * (Mathf.Sqrt(1 - time * time) + 1) + start;
                case TweenType.EaseInBounce:
                    end -= start;
                    return end - Evaluate(0, end, 1f - time, TweenType.EaseOutBounce) + start;
                case TweenType.EaseOutBounce:
                    end -= start;
                    if (time < 0.3637f)
                        return end * (7.5625f * time * time) + start;
                    if (time < 0.7272f)
                    {
                        time -= 0.5454f;
                        return end * (7.5625f * (time) * time + .75f) + start;
                    }
                    if (time < 0.9091f)
                    {
                        time -= 0.8182f;
                        return end * (7.5625f * (time) * time + .9375f) + start;
                    }
                    time -= 0.95454f;
                    return end * (7.5625f * (time) * time + .984375f) + start;
                case TweenType.EaseInOutBounce:
                    end -= start;
                    if (time < 0.5f) return Evaluate(0, end, time * 2, TweenType.EaseInBounce) * 0.5f + start;
                    return Evaluate(0, end, time * 2 - 1f, TweenType.EaseOutBounce) * 0.5f + end * 0.5f + start;
                case TweenType.EaseInBack:
                    end -= start;
                    return end * (time) * time * ((s1Const + 1) * time - s1Const) + start;
                case TweenType.EaseOutBack:
                    end -= start;
                    time = (time) - 1;
                    return end * ((time) * time * ((s1Const + 1) * time + s1Const) + 1) + start;
                case TweenType.EaseInOutBack:
                    end -= start;
                    time /= .5f;
                    if ((time) < 1)
                        return end * 0.5f * (time * time * (((s2Const) + 1) * time - s2Const)) + start;
                    time -= 2;
                    return end * 0.5f * ((time) * time * (((s2Const) + 1) * time + s2Const) + 2) + start;
                case TweenType.Punch:
                    if (time == 0)
                        return 0;
                    if (time == 1)
                        return 0;
                    return ((start + end) * Mathf.Pow(2, -10 * time) * Mathf.Sin((time) * (doublePI) / 0.3f));
                case TweenType.EaseInElastic:
                    end -= start;
                    if (time == 0) return start;
                    if (time == 1) return start + end;
                    return -(end * Mathf.Pow(2, 10 * (time - 1)) * Mathf.Sin((time - 0.075f) * (doublePI) / 0.3f)) + start;
                case TweenType.EaseOutElastic:
                    end -= start;
                    if (time == 0) return start;
                    if (time == 1) return start + end;
                    return (end * Mathf.Pow(2, -10 * time) * Mathf.Sin((time - 0.075f) * (doublePI) / 0.3f) + end + start);
                case TweenType.EaseInOutElastic:
                    end -= start;
                    if (time == 0) return start;
                    if ((time /= 0.5f) == 2) return start + end;
                    if (time < 1)
                        return -0.5f * (end * Mathf.Pow(2, 10 * (time -= 1)) * Mathf.Sin((time - 0.075f) * (doublePI) / 0.3f)) +
                               start;
                    return end * Mathf.Pow(2, -10 * (time -= 1)) * Mathf.Sin((time - 0.075f) * (doublePI) / 0.3f) * 0.5f + end +
                           start;
            }
            throw new System.Exception("Something Error!!!");
        }
    }
}
