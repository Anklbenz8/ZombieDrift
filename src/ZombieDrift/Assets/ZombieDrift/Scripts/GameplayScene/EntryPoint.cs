using Project;
using SaveLoadSystemNamespace;
using Unity.Cinemachine;
using UnityEngine;
using Zenject;

namespace Gameplay {
	public class EntryPoint : MonoBehaviour, IInitializable {
		private const float MOBILE_ORTHO_SIZE = 6.8f;
		private const float DESKTOP_ORTHO_SIZE = 9;
		
		[SerializeField] private Camera _camera;
		[SerializeField] private CinemachineCamera _mainCamera;
		[SerializeField] private CinemachineCamera _zoomCamera;
		[SerializeField] private MainMenuView _mainMenuView;
		[SerializeField] private LoseView _loseView;
		[SerializeField] private GameplayHudView _gameHudView;
		[SerializeField] private PauseView _pauseView;
		[SerializeField] private GetReadyView _getReadyView;
		[SerializeField] private HowToPlayView _howToPlayView;
		[SerializeField] private WinView winView;

		[SerializeField] private PointersView _pointersView;
		[SerializeField] private AudioListener _audioListener;
		private GameplayScenario _gameplayScenario;

		[Inject]
		private async void Construct(
				GameplayScenario gameplayScenario,
				CameraSystem cameraSystem,
				MenuPresenter menuPresenter,
				LosePresenter losePresenter,
				GameplayHudPresenter gameplayHudPresenter,
				PausePresenter pausePresenter,
				GetReadyPresenter getReadyPresenter,
				HowToPlayPresenter howToPlayPresenter,
				LevelCompletePresenter levelCompletePresenter,
				EnemyPointerSystem enemyPointerSystem,
				TextHintSystem textHintSystem,
				FlyingRewardSystem flyingRewardSystem,
				GameplaySounds gameplaySounds,
				UiSounds uiSounds,
				GameSettings gameSettings,
				SaveLoadSystem saveLoadSystem) {
			cameraSystem.mainCamera = _mainCamera;
			cameraSystem.zoomCamera = _zoomCamera;

			if (Application.isMobilePlatform)
				cameraSystem.mainCameraOrthoSize = MOBILE_ORTHO_SIZE;
			else 
				cameraSystem.mainCameraOrthoSize = DESKTOP_ORTHO_SIZE;
			
			menuPresenter.Initialize(_mainMenuView);
			losePresenter.Initialize(_loseView);
			gameplayHudPresenter.Initialize(_gameHudView);
			pausePresenter.Initialize(_pauseView);
			getReadyPresenter.Initialize(_getReadyView);
			howToPlayPresenter.Initialize(_howToPlayView);
			levelCompletePresenter.Initialize(winView);
			
			enemyPointerSystem.Initialize(_pointersView, _camera);
			flyingRewardSystem.Initialize(_camera, _gameHudView.rewardTargetTransform);
			textHintSystem.Initialize(_camera);

			gameplaySounds.Initialize(_audioListener);

			await saveLoadSystem.LoadObject(SaveType.PlayerPrefs, gameSettings);
			uiSounds.isMute = gameSettings.isMute;
			gameplaySounds.isMute = gameSettings.isMute;
			
			_gameplayScenario = gameplayScenario;
		}

		public void Initialize() =>
				_gameplayScenario.Start();

		/*//---- Кослыть для теста
		private async void Start() {

			await UniTask.Delay(2000);
			GP_Ads.OnRewardedReward += OnRewarded;
			GP_Ads.ShowRewarded();
		}
		private void OnRewarded(string arg0) {
			Debug.Log("По идее должа быть показана реклама!");
		}
		// --- костыть для теста*/
	}
}
