using UnityEngine;
using Project;
using UnityEngine.Localization;

namespace Gameplay {
    public class GameplayState : State {
        private const float SHAKE_AMPLITUDE = 0.5f;
        private const int SHAKE_DURATION = 85;
        private const int MAX_FLYING_COINS = 4;

        private const string LOCALIZE_TABLE = "StringsTable";
        private const string COMBO_HINT_LOCAL_KEY = "comboKey";

        private readonly StateSwitcher _stateSwitcher;
        private readonly GameplayHudPresenter _gameplayHudPresenter;
        private readonly GameProcess _gameProcess;
        private readonly VehicleController _vehicleController;
        private readonly VehicleDestroyer _vehicleDestroyer;
        private readonly CameraSystem _cameraSystem;
        private readonly BotNavigation _botNavigation;
        private readonly ParticlesPlayer _particlesPlayer;
        private readonly EnemyPointerSystem _enemyPointerSystem;
        private readonly MoneyWallet _moneyWallet;
        private readonly FlyingRewardSystem _flyingRewardSystem;
        private readonly ComboSystem _comboSystem;
        private readonly TextHintSystem _textHintSystem;
        private readonly GameplaySounds _gameplaySounds;
        private readonly ZombieStorage _zombieStorage;
        private readonly GameplayCache _gameplayCache;
        private readonly LocalizedString _comboLocalizedString;

        public GameplayState(StateSwitcher stateSwitcher,
            GameplayHudPresenter gameplayHudPresenter,
            GameProcess gameProcess,
            VehicleController vehicleController,
            VehicleDestroyer vehicleDestroyer,
            CameraSystem cameraSystem,
            BotNavigation botNavigation,
            ParticlesPlayer particlesPlayer,
            EnemyPointerSystem enemyPointerSystem,
            MoneyWallet moneyWallet,
            FlyingRewardSystem flyingRewardSystem,
            ComboSystem comboSystem,
            TextHintSystem textHintSystem,
            GameplaySounds gameplaySounds,
            ZombieStorage zombieStorage,
            GameplayCache gameplayCache) : base(stateSwitcher) {
            _stateSwitcher = stateSwitcher;
            _gameplayHudPresenter = gameplayHudPresenter;
            _gameProcess = gameProcess;
            _vehicleController = vehicleController;
            _vehicleDestroyer = vehicleDestroyer;
            _cameraSystem = cameraSystem;
            _botNavigation = botNavigation;
            _particlesPlayer = particlesPlayer;
            _enemyPointerSystem = enemyPointerSystem;
            _moneyWallet = moneyWallet;
            _flyingRewardSystem = flyingRewardSystem;
            _comboSystem = comboSystem;
            _textHintSystem = textHintSystem;
            _gameplaySounds = gameplaySounds;
            _zombieStorage = zombieStorage;
            _gameplayCache = gameplayCache;
            _comboLocalizedString = new LocalizedString(LOCALIZE_TABLE, COMBO_HINT_LOCAL_KEY);
        }

        public override void Enter() {
            _gameplayHudPresenter.presentState = StagePresentState.All;
            _gameplayHudPresenter.viewActions.PauseClickedEvent += SwitchToPauseState;
    

            _vehicleController.Start();
            _botNavigation.Start();
            _enemyPointerSystem.enabled = true;
            _gameplaySounds.StartCarSounds();
            _gameplaySounds.StartZombieVoices(_zombieStorage);

            //     _comboCounter.comboDelay = COMBO_ACTIVE_TIME;
            _flyingRewardSystem.CollectedEvent += OnFlyingRewardArrived;

            _gameProcess.ObstacleHitEvent += OnCarHitObstacle;
            _gameProcess.AllEnemiesDestroyedEvent += SwitchToWinState;
            _gameProcess.ZombieHitEvent += OnEnemyHit;
        }
        
        public override void Exit() {
            _gameplayHudPresenter.presentState = StagePresentState.None;
            _gameplayHudPresenter.viewActions.PauseClickedEvent -= SwitchToPauseState;

            _vehicleController.Stop();
            _botNavigation.Stop();
            _enemyPointerSystem.enabled = false;

            _gameplaySounds.StopCarSounds();
            _gameplaySounds.StopZombieVoices();

            _flyingRewardSystem.CollectedEvent -= OnFlyingRewardArrived;
            _gameProcess.ObstacleHitEvent -= OnCarHitObstacle;
            _gameProcess.AllEnemiesDestroyedEvent -= SwitchToWinState;
            _gameProcess.ZombieHitEvent -= OnEnemyHit;
        }

        public override void FixedTick() {
            _botNavigation.Tick();
            _enemyPointerSystem.Tick();
            _comboSystem.TimerRefresh();
            _gameplaySounds.UpdateCarSounds(_vehicleController.carPosition, _vehicleController.normalizedVelocity,
                Mathf.Abs(_vehicleController.wheelsAxisHorizontal));
        }

        private async void OnEnemyHit(Zombie zombie) {
            var hitPosition = zombie.position;

            _gameplaySounds.PlayZombieHitSoundAtPosition(hitPosition);
            _particlesPlayer.PlayZombieHit(hitPosition);
            _enemyPointerSystem.Remove(zombie);

           // var coinsReward = _gameplayCache.comboMultiplier;
           // var flyingCoinsCount = Mathf.Min(MAX_FLYING_COINS, coinsReward);
            _moneyWallet.AddCoins();
            _flyingRewardSystem.SpawnInSphere(hitPosition);

            TryGetComboReward(hitPosition);

            await _cameraSystem.Shake(SHAKE_AMPLITUDE, SHAKE_DURATION);
        }

        private void OnCarHitObstacle(Vector3 point) {
            _gameplaySounds.PlayCarCrashSoundAtPosition(point);
            _particlesPlayer.PlayObstacleHit(point);
            _vehicleDestroyer.DestroyFormPoint(point);
            SwitchToLoseState();
        }

        private void TryGetComboReward(Vector3 hitPosition) {
            var comboCount = _comboSystem.GetIncreasedComboCount();
            if (comboCount == 0)
                return;

            //Ограничичваем число монет которые генерятся после сбития - чтобы не летело с каждого по 10 монет - но и визуально
         //  var flyingCoinsCount = Mathf.Min(MAX_FLYING_COINS, comboCount);
             var comboRewardCount = comboCount * _gameplayCache.comboMultiplier;
            _flyingRewardSystem.SpawnInSphere(hitPosition, comboRewardCount);
            _moneyWallet.AddCoins(comboRewardCount);

            _textHintSystem.ShowHint(hitPosition, _comboLocalizedString.GetLocalizedString(_comboSystem.count));
        }

        private void OnFlyingRewardArrived() =>
            _gameplayHudPresenter.IncreaseMoneyCount();

        private void SwitchToWinState() =>
            _stateSwitcher.SetState<WinState>();

        private void SwitchToLoseState() =>
            _stateSwitcher.SetState<LoseState>();

        private void SwitchToPauseState() =>
            _stateSwitcher.SetState<PauseState>();
    }
}