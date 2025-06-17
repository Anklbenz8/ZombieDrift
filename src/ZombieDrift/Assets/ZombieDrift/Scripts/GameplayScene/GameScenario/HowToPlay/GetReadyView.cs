using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
	public class GetReadyView : View {
		public event Action BackClickedEvent;
		[SerializeField] private Button _backButton;

		private void BackNotify() =>
				BackClickedEvent?.Invoke();

		private void OnEnable() =>
				_backButton.onClick.AddListener(BackNotify);

		private void OnDisable() =>
				_backButton.onClick.RemoveListener(BackNotify);
	}
}
