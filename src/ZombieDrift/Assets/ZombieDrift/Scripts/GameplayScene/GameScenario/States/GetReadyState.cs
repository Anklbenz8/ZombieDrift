using Project;
using UnityEngine;

namespace Gameplay {
	public class GetReadyState : State {
		private readonly StateSwitcher _stateSwitcher;
		private readonly GetReadyPresenter _getReadyPresenter;
		private readonly HowToPlayPresenter _howToPlayPresenter;
		private readonly GameplayHudPresenter _gameplayHudPresenter;

		public GetReadyState(StateSwitcher stateSwitcher,
				GetReadyPresenter getReadyPresenter,
				HowToPlayPresenter howToPlayPresenter,
				GameplayHudPresenter gameplayHudPresenter) : base(stateSwitcher) {
			_stateSwitcher = stateSwitcher;
			_getReadyPresenter = getReadyPresenter;
			_howToPlayPresenter = howToPlayPresenter;
			_gameplayHudPresenter = gameplayHudPresenter;
		}

		public override void Enter() {
			_gameplayHudPresenter.presentState = StagePresentState.AllWithoutPause;
			_howToPlayPresenter.enabled = true;
			_getReadyPresenter.enabled = true;
			_getReadyPresenter.GoToMenuEvent += SwitchToMainMenu;
			_getReadyPresenter.GoToPlayEvent += SwitchToPlayState;
		}

		public override void Exit() {
			_gameplayHudPresenter.presentState = StagePresentState.None;
			_getReadyPresenter.enabled = false;
			_getReadyPresenter.GoToMenuEvent -= SwitchToMainMenu;
			_getReadyPresenter.GoToPlayEvent -= SwitchToPlayState;
		}

		private void SwitchToMainMenu() {
			_howToPlayPresenter.enabled = false;
			_stateSwitcher.SetState<MenuState>();
		}

		private void SwitchToPlayState() {
			_howToPlayPresenter.HideWithDelay();
			_stateSwitcher.SetState<GameplayState>();
		}
	}
}
