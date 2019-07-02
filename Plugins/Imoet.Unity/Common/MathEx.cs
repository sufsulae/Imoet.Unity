using UnityEngine;
namespace Imoet.Unity
{
    public static class MathEx {
        private static System.Random _random;
        static MathEx() {
            _random = new System.Random();
        }
        public static float LerpPercent(float value, float from, float to)
        {
            return (value - from) / (to - from);
        }
        public static float RandomRange(float min, float max)
        {
            float amount = (float)_random.NextDouble();
            return Mathf.Lerp(min, max, amount);
        }
        public static float LoopNum(float start, float end, float value)
        {
            float diff = end - start;
            while (value < start)
                value += diff;
            while (value > end)
                value -= diff;
            return value;
        }
        public static float Barycentric(float val1, float val2, float val3, float amnt1, float amnt2)
        {
            return val1 + (val2 - val1) * amnt1 + (val3 - val2) * amnt2;
        }
        public static float CatmullRom(float val1, float val2, float val3, float val4, float amount)
        {
            var sqr = amount * amount;
            var cube = sqr * amount;
            return (0.5f * (2.0f * val2 + (val3 - val1) * amount + (2.0f * val1 - 5.0f * val2 + 4.0f * val3 - val4) * sqr + (3.0f * val2 - val1 - 3.0f * val3 + val4) * cube));
        }
        public static float Hermite(float val1, float tan1, float val2, float tan2, float amount)
        {
            float sqr = amount * amount;
            float cube = sqr * amount;
            amount = Mathf.Clamp01(amount);
            if (amount == 0f)
                return val1;
            else if (amount == 1f)
                return val2;
            else
                return (2 * val1 - 2 * val2 + tan2 + tan1) * cube + (3 * val2 - 3 * val1 - 2 * tan1 - tan2) * sqr + tan1 * amount + val1;
        }
    }
}
