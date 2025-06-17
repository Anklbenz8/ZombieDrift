using System;
using Project;
namespace Gameplay {
	public class LevelCompletePresenter {
		private readonly UiSounds _uiSounds;
		public event Action ContinueEvent;

		private WinView _view;

		public LevelCompletePresenter(UiSounds uiSounds) {
			_uiSounds = uiSounds;
		}

		public bool enabled {
			set {
				if (value)
					_uiSounds.PlayWinSound();
				_view.isActive = value;
			}
		}

		public string completeMessage {
			set => _view.mapText = value;
		}

		public void Initialize(WinView winView) {
			_view = winView;
			_view.ContinueButtonClickedEvent += ContinueNotify;
		}
		private void ContinueNotify() {
			_uiSounds.PlayClickSound();
			ContinueEvent?.Invoke();
		}
	}
}
