namespace Imoet.Unity.Security.SafeValue
{
    public class SafeDecimal : Internal.SafeNumber<decimal>
    {
        public SafeDecimal() : this(0.0m){ }
        public SafeDecimal(decimal num) : base(){
            _value_ = num;
        }

        protected override decimal _onGetValue(decimal encodedValue, int salt)
        {
            return encodedValue - salt;
        }
        protected override decimal _onSetValue(decimal valueInput, int salt)
        {
            return valueInput + salt;
        }

        public static implicit operator SafeDecimal(decimal num) {
            return new SafeDecimal(num);
        }
        public static implicit operator decimal(SafeDecimal num)
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
            if (obj is decimal) {
                return _value_ == (decimal)obj;
            }
            return base.Equals(obj);
        }
    }
}
