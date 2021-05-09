using System;

namespace Imoet.Unity.Animation
{
    [Serializable]
    public class FromTo<T>
    {
        public T from;
        public T to;

        public FromTo() {
            from = default(T);
            to = default(T);
        }

        public FromTo(T from, T to) {
            this.from = from;
            this.to = to;
        }
    }

    [Serializable]
    public class FromToSetting<T> : FromTo<T> {
        public TweenSetting setting;
    }
}
