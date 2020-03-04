namespace Imoet.Unity.Security.SafeValue
{
    public class SafeLong : Internal.SafeNumber<long>
    {
        public SafeLong() : this(0){ }
        public SafeLong(long num) : base() {
            _value_ = num;
        }

        protected override long _onGetValue(long encodedValue, int salt) {
            return encodedValue - salt;
        }
        protected override long _onSetValue(long inputValue, int salt) {
            return inputValue + salt;
        }

        public static implicit operator SafeLong(long num) {
            return new SafeLong(num);
        }
        public static implicit operator long(SafeLong num)
        {
            return num._value_;
        }

        public override int GetHashCode()
        {
            return _value_.GetHashCode();
        }
        public override string ToString()
        {
            return _value_.ToString();
        }
        public override bool Equals(object obj)
        {
            if (obj is long) {
                return _value_ == (long)obj;
            }
            return base.Equals(obj);
        }
    }
}
