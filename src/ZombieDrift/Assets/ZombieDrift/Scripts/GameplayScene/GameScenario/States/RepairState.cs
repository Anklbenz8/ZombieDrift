using Project;
using UnityEngine;

namespace Gameplay {

	public class RepairState : State {
		private readonly StateSwitcher _stateSwitcher;
		private readonly GameplayCache _gameplayCache;
		private readonly Progress _progress;
		private readonly VehicleDestroyer _vehicleDestroyer;
		private readonly VehicleController _vehicleController;
		private readonly ContentCreationService _contentCreationService;
		private readonly CameraSystem _cameraSystem;
		private readonly GameProcess _gameProcess;
		private readonly BotNavigation _botNavigation;
		private readonly EnemyPointerSystem _enemyPointerSystem;

		public RepairState(
				StateSwitcher stateSwitcher,
				GameplayCache gameplayCache,
				Progress progress,
				VehicleDestroyer vehicleDestroyer,
				VehicleController vehicleController,
				ContentCreationService contentCreationService,
				CameraSystem cameraSystem,
				GameProcess gameProcess,
				BotNavigation botNavigation,
				EnemyPointerSystem enemyPointerSystem) : base(stateSwitcher) {
			_stateSwitcher = stateSwitcher;
			_progress = progress;
			_gameplayCache = gameplayCache;
			_vehicleDestroyer = vehicleDestroyer;
			_vehicleController = vehicleController;
			_contentCreationService = contentCreationService;
			_cameraSystem = cameraSystem;
			_gameProcess = gameProcess;
			_botNavigation = botNavigation;
			_enemyPointerSystem = enemyPointerSystem;
		}

		public override void Enter() {
			DestroyCarObject();
			CreateAndInitializeNewCar();
			SwitchToGameplayState();
		}

		private void DestroyCarObject() {
			Object.Destroy(_gameplayCache.car.gameObject);
			_gameplayCache.car = null;
		}

		private void CreateAndInitializeNewCar() {
			var map = _gameplayCache.map;
			var car = _contentCreationService.CreateCar(_progress.selectedCarIndex, map.startPoint);
			var startPose = map.startPoint;
			car.transform.position = startPose.position;
			car.transform.rotation = startPose.rotation;
			car.Initialize();

			_cameraSystem.target = car.transform;
			_vehicleController.SetCar(car);
			_vehicleDestroyer.SetCar(car);
			_botNavigation.target = car.transform;
			_gameProcess.SetCar(car);
			_enemyPointerSystem.player = car.transform;
			_gameplayCache.car = car;
		}

		private void SwitchToGameplayState() {
			_stateSwitcher.SetState<GetReadyState>();
		}
	}
}
