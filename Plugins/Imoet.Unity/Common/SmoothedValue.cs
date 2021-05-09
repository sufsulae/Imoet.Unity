namespace Imoet.Unity
{
    public interface ISmoothedValue<T> {
		T value { get; set; }
		T smoothedValue { get; }
		void UpdateSmooth();
	}

	[System.Serializable]
	public abstract class SmoothedValue<T> {
		public T value {
			get { return _value; }
			set { _value = value; }
		}
		public T smoothedValue {
			get { return _smoothedValue; }
		}
		public float damping { get; set; }

		protected T _value;
		protected T _smoothedValue;

		public SmoothedValue() {
			_value = default(T);
			_smoothedValue = default(T);
		}
		public SmoothedValue(T value){
			_value = value;
			_smoothedValue = value;
		}
	}
}
