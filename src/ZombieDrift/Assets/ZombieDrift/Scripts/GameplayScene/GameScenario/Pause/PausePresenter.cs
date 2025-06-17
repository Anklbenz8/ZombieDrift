using Project;

namespace Gameplay {
	public class PausePresenter {
		private readonly UiSounds _uiSounds;
		public IPauseViewEvents view => _view;

		private PauseView _view;

		public PausePresenter(UiSounds uiSounds) {
			_uiSounds = uiSounds;
		}
		
		public bool enabled {
			set {
				_view.isActive = value;
				_uiSounds.PlayClickSound();
			}
		}

		public void Initialize(PauseView pauseView) =>
				_view = pauseView;

	}
}
