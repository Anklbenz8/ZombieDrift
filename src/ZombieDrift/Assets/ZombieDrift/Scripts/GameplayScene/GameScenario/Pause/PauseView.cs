using System;
using UnityEngine;
using UnityEngine.UI;
namespace Gameplay {
	public interface IPauseViewEvents {
		event Action ContinueEvent, MuteChangedEvent;
		bool isMute { set; }
	}

	public class PauseView : View, IPauseViewEvents {
		public event Action ContinueEvent, MuteChangedEvent;
		[SerializeField] private Image _muteImage, _unmuteImage;
		[SerializeField] private Button _continueButton, _muteButton;

		public bool isMute {
			set {
				_unmuteImage.enabled = !value;
				_muteImage.enabled = value;
			}
		}
		private void ContinueNotify() =>
				ContinueEvent?.Invoke();

		private void MuteUnmuteNotify() =>
				MuteChangedEvent?.Invoke();

		private void OnEnable() {
			_muteButton.onClick.AddListener(MuteUnmuteNotify);
			_continueButton.onClick.AddListener(ContinueNotify);
		}
		private void OnDisable() {
			_muteButton.onClick.RemoveListener(MuteUnmuteNotify);
			_continueButton.onClick.RemoveListener(ContinueNotify);
		}
	}
}
