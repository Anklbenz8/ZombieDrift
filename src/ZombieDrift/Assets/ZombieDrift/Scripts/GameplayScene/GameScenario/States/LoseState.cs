using Ads;
using Project;
using SaveLoadSystemNamespace;
using UnityEngine;

namespace Gameplay {
	public class LoseState : State {
		private const int REPAIR_BASE_COST = 25;

		private readonly StateSwitcher _stateSwitcher;
		private readonly SaveLoadSystem _saveLoadSystem;
		private readonly GameplayCache _gameplayCache;
		private readonly LosePresenter _losePresenter;
		private readonly CameraSystem _cameraSystem;
		private readonly GameplaySounds _gameplaySounds;
		private readonly PauseService _pauseService;
		private readonly AdsSystem _adsSystem;
		private readonly GameplayHudPresenter _gameplayHudPresenter;
		private readonly Progress _progress;
		private readonly MoneyWallet _moneyWallet;

		private int repairCost => REPAIR_BASE_COST * _gameplayCache.loseInCurrentStageCount;
		private bool _isMutedWhenAdsRequested;
		public LoseState(
				StateSwitcher stateSwitcher,
				SaveLoadSystem saveLoadSystem,
				GameplayCache gameplayCache,
				LosePresenter losePresenter,
				CameraSystem cameraSystem,
				GameplaySounds gameplaySounds,
				PauseService pauseService,
				AdsSystem adsSystem,
				GameplayHudPresenter gameplayHudPresenter,
				Progress progress,
				MoneyWallet moneyWallet) : base(stateSwitcher) {
			_stateSwitcher = stateSwitcher;
			_saveLoadSystem = saveLoadSystem;
			_gameplayCache = gameplayCache;
			_losePresenter = losePresenter;
			_cameraSystem = cameraSystem;
			_gameplaySounds = gameplaySounds;
			_pauseService = pauseService;
			_adsSystem = adsSystem;
			_gameplayHudPresenter = gameplayHudPresenter;
			_progress = progress;
			_moneyWallet = moneyWallet;
		}


		public override void Enter() {
			_gameplayCache.loseInCurrentStageCount++;
			ShowCameraActions();
			SaveGame();
			//  _isMutedWhenAdsRequested = _gameplaySounds.isMute;
			//   _gameplaySounds.isMute = true;

			_gameplayHudPresenter.presentState = StagePresentState.MoneyOnly;

			_losePresenter.enabled = true;
			_losePresenter.repairCost = repairCost;
			_losePresenter.isRepairByMoneyInteractable = repairCost <= _moneyWallet.count;
			var r = _adsSystem.isRewardedAvailable;
			Debug.Log($"Reward avaiable {r}");
			_losePresenter.isRepairByAdsInteractable = r;
			
			_losePresenter.RepairByAdsEvent += OnRepairByAdsClicked;
			_losePresenter.RepairByMoneyEvent += OnRepairByMoneyClicked;
			_losePresenter.RestartEvent += OnStartFromScratchClicked;
		}

		public override void Exit() {
			_gameplayHudPresenter.presentState = StagePresentState.None;
			//     _gameplaySounds.isMute = _isMutedWhenAdsRequested;

			_losePresenter.enabled = false;
			_losePresenter.RepairByAdsEvent -= OnRepairByAdsClicked;
			_losePresenter.RepairByMoneyEvent -= OnRepairByMoneyClicked;
			_losePresenter.RestartEvent -= OnStartFromScratchClicked;
			_cameraSystem.isZoomed = false;
		}

		private void SaveGame() =>
#if !UNITY_EDITOR && UNITY_WEBGL
			_saveLoadSystem.SaveObject(SaveType.GamePushCloud, _progress);
#else
				_saveLoadSystem.SaveObject(SaveType.PlayerPrefs, _progress);
#endif
		// _saveLoadSystem.SaveObject(SaveType.GamePushCloud, _progress);
		// _saveLoadSystem.SaveObject(SaveType.PlayerPrefs, _progress);

		private async void ShowCameraActions() {
			await _cameraSystem.Shake(1, 250);
			_cameraSystem.isZoomed = true;
		}

		private async void OnRepairByAdsClicked() {
			_losePresenter.isRepairByAdsInteractable = false;
			_gameplaySounds.isMute = true;

			var rewardCollected = await _adsSystem.ShowRewardVideo();

			if (rewardCollected)
				SwitchToRepairState();

			_gameplaySounds.isMute = false;
			_losePresenter.isRepairByAdsInteractable = true;
		}

		private void OnRepairByMoneyClicked() {
			_moneyWallet.SpendCoin(repairCost);
			_gameplayHudPresenter.moneyCount = _moneyWallet.count;
			SaveGame();
			SwitchToRepairState();
		}

		private void OnStartFromScratchClicked() {
			_gameplayCache.mapIndex = 0;
			_gameplayCache.loseInCurrentStageCount = 0;
			//    _adsSystem.ShowFullscreen();
			SwitchToRestartState();
		}

		private void SwitchToRestartState() =>
				_stateSwitcher.SetState<FinalizeState>();

		private void SwitchToRepairState() =>
				_stateSwitcher.SetState<RepairState>();
	}
}
