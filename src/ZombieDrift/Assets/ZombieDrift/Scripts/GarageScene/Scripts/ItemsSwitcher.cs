using System;
using System.Collections.Generic;

namespace Garage {
	public class ItemsSwitcher {
		public event Action BeforeSelectEvent, SelectedChangedEvent;
		public GarageItem selected => _garageData[_currentIndex];

		public int currentIndex {
			get => _currentIndex;
			private set {
				_currentIndex = value;
				SelectedChangedEvent?.Invoke();
			}
		}

		private GarageItem[] _garageData;
		private int _currentIndex;

		public void SetData(GarageItem[] garageData) {
			_garageData = garageData;
		}

		public void Select(int index) {
			BeforeSelectEvent?.Invoke();
			currentIndex = index;
		}

		public void MoveNext() {
			var nextIndex = _currentIndex + 1;
			var isIndexValid = CheckIndexIsValid(nextIndex);
			if (!isIndexValid)
				return;
			Select(nextIndex);
		}

		public void MovePrevious() {
			var previousIndex = _currentIndex - 1;
			var isIndexValid = CheckIndexIsValid(previousIndex);
			if (!isIndexValid)
				return;
			Select(previousIndex);
		}

		private bool CheckIndexIsValid(int index)
			=> index >= 0 && index < _garageData.Length;
	}
}
