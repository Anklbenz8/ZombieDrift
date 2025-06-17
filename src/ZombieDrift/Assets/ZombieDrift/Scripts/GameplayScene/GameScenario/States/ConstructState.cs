using System.Linq;
using Project;
using UnityEngine;

namespace Gameplay {
	public class ConstructState : State {

		private readonly ContentCreationService _contentCreationService;
		private readonly Progress _progress;
		private readonly GameplayCache _gameplayCache;
		private readonly MoneyWallet _moneyWallet;
		private readonly ComboSystem _comboSystem;
		private readonly CarsConfig _carsConfig;
		private readonly ZombieStorage _zombieStorage;
		private readonly PauseService _pauseService;
		private readonly BotNavigation _botSystem;
		private readonly EnemyPointerSystem _enemyPointerSystem;
		private readonly StagesConfig _stagesConfig;
		private readonly GameProcess _gameProcess;
		private readonly CameraSystem _cameraSystem;
		private readonly StateSwitcher _stateSwitcher;
		private readonly VehicleController _vehicleController;
		private readonly VehicleDestroyer _vehicleDestroyer;
		private readonly FlyingRewardSystem _flyingRewardSystem;
		private readonly GameplayHudPresenter _gameplayHudPresenter;

		public ConstructState(
				StateSwitcher stateSwitcher,
				ContentCreationService contentCreationService,
				CameraSystem cameraSystem,
				GameProcess gameProcess,
				VehicleController vehicleController,
				VehicleDestroyer vehicleDestroyer,
				FlyingRewardSystem flyingRewardSystem,
				GameplayHudPresenter gameplayHudPresenter,
				BotNavigation botSystem,
				EnemyPointerSystem enemyPointerSystem,
				StagesConfig stagesConfig,
				Progress progress,
				GameplayCache gameplayCache,
				MoneyWallet moneyWallet,
				ComboSystem comboSystem,
				CarsConfig carsConfig,
				ZombieStorage zombieStorage,
				PauseService pauseService
		) : base(stateSwitcher) {
			_stateSwitcher = stateSwitcher;
			_contentCreationService = contentCreationService;
			_cameraSystem = cameraSystem;
			_gameProcess = gameProcess;
			_vehicleController = vehicleController;
			_vehicleDestroyer = vehicleDestroyer;
			_flyingRewardSystem = flyingRewardSystem;
			_gameplayHudPresenter = gameplayHudPresenter;
			_progress = progress;
			_gameplayCache = gameplayCache;
			_moneyWallet = moneyWallet;
			_comboSystem = comboSystem;
			_carsConfig = carsConfig;
			_zombieStorage = zombieStorage;
			_pauseService = pauseService;
			_botSystem = botSystem;
			_enemyPointerSystem = enemyPointerSystem;
			_stagesConfig = stagesConfig;
		}

		public override async void Enter() {
			CalculateCurrentComboMultiplier();
			LoadGameplayCache();
			CreateGameplayObjects();
			SnapCameraToCar();
			InitializeGameplay();
			InitializePause();

			var mapsCount = _gameplayCache.mapsCount;
			var mapIndex = _gameplayCache.mapIndex;
			
			//_gameplayHudPresenter.stageIndex = _progress.stageIndex;
			await _gameplayHudPresenter.SetStageIndex(_progress.stageIndex);
			_gameplayHudPresenter.mapIndex = new Vector2Int(mapIndex, mapsCount);
			_gameplayHudPresenter.moneyCount = _moneyWallet.count;
			
			if (mapIndex == 0)
				SwitchToMenuState();
			else
				SwitchToHowToPlayState();
		}
		private void InitializePause() {
			_pauseService.Register(_vehicleController);
			_pauseService.Register(_flyingRewardSystem);
		}
		
		private void CalculateCurrentComboMultiplier() {
			var purchasedCars = _progress.purchasedCars;
			//_comboSystem.comboMultiplier = purchasedCars.Count;
			_gameplayCache.comboMultiplier = purchasedCars.Count;
			_comboSystem.Reset();
		}

		private void CreateGameplayObjects() {
			var stageIndex = _progress.stageIndex;
			var currentCarIndex = _progress.selectedCarIndex;
			var mapIndex = _gameplayCache.mapIndex;
			var map = _contentCreationService.CreateMap(stageIndex, mapIndex);
			
			map.navMeshSurface.BuildNavMesh();
			_gameplayCache.map = map;
			_gameplayCache.car = _contentCreationService.CreateCar(currentCarIndex, map.startPoint);
			var zombies = _contentCreationService.CreateZombies(map.zombieSpawnPoints, map.transform);
			_zombieStorage.AddNewRange(zombies);
		}

		private void LoadGameplayCache() {
			var stageIndex = _progress.stageIndex;

			//--- !!!---- Цикличность Когда уровни закончатся начнутся в сначала
			if (stageIndex >= _stagesConfig.stages.Length) {
				_progress.stageIndex = 0;
				stageIndex = 0;
			}
			//
			_gameplayCache.mapsCount = _stagesConfig.stages[stageIndex].count;
		}

		private void SnapCameraToCar() =>
				_cameraSystem.target = _gameplayCache.car.transform;

		private void InitializeGameplay() {
			var car = _gameplayCache.car;
			car.Initialize();

			_vehicleController.SetCar(car);
			_vehicleDestroyer.SetCar(car);

			_botSystem.Initialize(_zombieStorage, car.transform);
			_gameProcess.Initialize(car, _zombieStorage);
			_enemyPointerSystem.SetNewData(_zombieStorage.ToArray(), car.transform);
		}

		private void SwitchToMenuState() =>
				_stateSwitcher.SetState<MenuState>();

		private void SwitchToHowToPlayState() =>
				_stateSwitcher.SetState<GetReadyState>();
	}
}
