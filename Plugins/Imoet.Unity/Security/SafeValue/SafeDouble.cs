namespace Imoet.Unity.Security.SafeValue
{
    public class SafeDouble : Internal.SafeNumber<decimal>
    {
        public SafeDouble() : this(0.0d){ }
        public SafeDouble(double num) : base() {
            _value_ = (decimal)num;
        }

        protected override decimal _onGetValue(decimal encodedValue, int salt) {
            return encodedValue - salt;
        }
        protected override decimal _onSetValue(decimal valueInput, int salt) {
            return valueInput + salt;
        }

        public static implicit operator SafeDouble(double num) {
            return new SafeDouble(num);
        }
        public static implicit operator double(SafeDouble num)
        {
            return (double)num._value_;
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
            if (obj is double) {
                return (double)_value_ == (double)obj;
            }
            return base.Equals(obj);
        }
    }
}
