using Project;
using SaveLoadSystemNamespace;
using UnityEngine;

namespace Gameplay {
	public class PauseState : State {
		private readonly StateSwitcher _stateSwitcher;
		private readonly PausePresenter _pausePresenter;
		private readonly UiSounds _uiSounds;
		private readonly GameplaySounds _gameplaySounds;
		private readonly GameSettings _gameSettings;
		private readonly SaveLoadSystem _saveLoadSystem;
		private readonly PauseService _pauseService;

		public PauseState(StateSwitcher stateSwitcher,
				PausePresenter pausePresenter,
				UiSounds uiSounds,
				GameplaySounds gameplaySounds,
				GameSettings gameSettings,
				SaveLoadSystem saveLoadSystem,
				PauseService pauseService
		) : base(stateSwitcher) {
			_stateSwitcher = stateSwitcher;
			_pausePresenter = pausePresenter;
			_uiSounds = uiSounds;
			_gameplaySounds = gameplaySounds;
			_gameSettings = gameSettings;
			_saveLoadSystem = saveLoadSystem;
			_pauseService = pauseService;
		}

		public override void Enter() {
			_pauseService.SetPause(true);

			_pausePresenter.enabled = true;
			_pausePresenter.view.isMute = _gameSettings.isMute;

			_pausePresenter.view.ContinueEvent += SwitchToGameplay;
			_pausePresenter.view.MuteChangedEvent += ChangeMute;
		}

		public override void Exit() {
			_pauseService.SetPause(false);

			_pausePresenter.enabled = false;
			_pausePresenter.view.ContinueEvent -= SwitchToGameplay;
			_pausePresenter.view.MuteChangedEvent -= ChangeMute;
		}

		private void ChangeMute() {
			_gameSettings.isMute = !_gameSettings.isMute;
			var isMuted = _gameSettings.isMute;

			_pausePresenter.view.isMute = isMuted;
			_uiSounds.isMute = isMuted;
			_gameplaySounds.isMute = isMuted;

			_saveLoadSystem.SaveObject(SaveType.PlayerPrefs, _gameSettings);
		}

		private void SwitchToGameplay() =>
				_stateSwitcher.SetState<GameplayState>();
	}
}
