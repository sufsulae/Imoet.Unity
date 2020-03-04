namespace Imoet.Unity.Security.SafeValue
{
    public class SafeShort : Internal.SafeNumber<short>
    {
        public SafeShort() : this(0){ }
        public SafeShort(short num) : base(){
            _value_ = num;
        }

        protected override short _onGetValue(short encodedValue, int salt)
        {
            return (short)(encodedValue - salt);
        }
        protected override short _onSetValue(short valueInput, int salt)
        {
            return (short)(valueInput + salt);
        }

        public static implicit operator SafeShort(short num) {
            return new SafeShort(num);
        }
        public static implicit operator short(SafeShort num)
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
            if (obj is short) {
                return _value_ == (short)obj;
            }
            return base.Equals(obj);
        }
    }
}
