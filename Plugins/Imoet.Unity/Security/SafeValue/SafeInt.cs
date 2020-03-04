namespace Imoet.Unity.Security.SafeValue
{
    public class SafeInt : Internal.SafeNumber<int>
    {
        public SafeInt() : this(0){ }
        public SafeInt(int num) : base() {
            _value_ = num;
        }

        protected override int _onGetValue(int encodedValue, int salt) {
            return encodedValue - salt;
        }
        protected override int _onSetValue(int valueInput, int salt) {
            return valueInput + salt;
        }

        public static implicit operator SafeInt(int num) {
            return new SafeInt(num);
        }
        public static implicit operator int(SafeInt num)
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
            if (obj is int) {
                return _value_ == (int)obj;
            }
            return base.Equals(obj);
        }
    }
}
