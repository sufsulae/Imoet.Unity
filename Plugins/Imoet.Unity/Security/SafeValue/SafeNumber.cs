using System;
namespace Imoet.Unity.Security.SafeValue.Internal
{
    public abstract class SafeNumber<T>
    {
        private int m_salt;
        public SafeNumber() {
            var type = typeof(T);
            if (!type.Equals(typeof(decimal))) {
                if (!type.IsPrimitive || type.Equals(typeof(string)))
                    throw new ArgumentException("SafeNumber class is not compatible with this kind of data type: " + type);
            }
            m_salt = SafeNumberManager.GetSalt<T>();
        }

        protected T encodedValue;
        protected virtual T _value_ {
            get {
                return _onGetValue(encodedValue,m_salt);
            } 
            set {
                encodedValue = _onSetValue(value, m_salt);
            }
        }
        protected virtual T _onGetValue(T encodedValue, int salt) {
            return default(T);
        }
        protected virtual T _onSetValue(T inputValue, int salt) {
            return default(T);
        }

        ~SafeNumber() {
            SafeNumberManager.RemoveSalt<T>(m_salt);
        }
    }
}
