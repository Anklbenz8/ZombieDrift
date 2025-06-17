using UnityEngine;
namespace Gameplay {
	public class ComboSystem {
		private const int MIN_COMBO_COUNT_FOR_NOTIFY = 3;
		private const float DEFAULT_COMBO_DELAY = 1f;

		//public float comboMultiplier { get; set; }
		public float comboDelay { get; set; } = DEFAULT_COMBO_DELAY;
		public int count => _count;

		private readonly Timer _timer;

		private int _count;

		public ComboSystem() {
			_timer = new Timer();
			_timer.AlarmEvent += Reset;
		}

		public int GetIncreasedComboCount() {
			var comboCount = IncreaseCombo();
			return comboCount >= MIN_COMBO_COUNT_FOR_NOTIFY
					? comboCount
					//? Mathf.RoundToInt(comboCount * comboMultiplier)
					: 0;
		}

		private int IncreaseCombo() {
			++_count;
			_timer.StartWithAlarm(comboDelay);
			return _count;
		}

		public void TimerRefresh() =>
				_timer.Tick();

		public void Reset() =>
				_count = 0;

		~ComboSystem() =>
				_timer.AlarmEvent -= Reset;
	}
}
