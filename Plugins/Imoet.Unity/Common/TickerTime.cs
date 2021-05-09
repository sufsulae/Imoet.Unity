using System;

namespace Imoet.Unity {
	public class TickerTime {
		public float duration { get; set; }
		public float timeStep { get; set; }
		public float tickerTime { get; set; }

		public Action OnTicking { get; set; }

		public void Update() {
			duration += timeStep;
			if (duration > tickerTime) {
				duration = 0;
				OnTicking?.Invoke();
			}
		}
	}
}
