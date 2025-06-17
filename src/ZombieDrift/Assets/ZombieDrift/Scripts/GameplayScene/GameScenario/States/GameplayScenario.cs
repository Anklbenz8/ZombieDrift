using Project;

namespace Gameplay {
    public class GameplayScenario {
        private readonly StateSwitcher _stateSwitcher;

        public GameplayScenario(
            StateSwitcher stateSwitcher,
            ConstructState constructState,
            MenuState menuState,
            GetReadyState getReadyState,
            GameplayState gameplayState,
            PauseState pauseState,
            WinState winState,
            LoseState loseState,
            RepairState repairState,
            FinalizeState finalizeState) {
            _stateSwitcher = stateSwitcher;
            _stateSwitcher.AddState(constructState);
            _stateSwitcher.AddState(menuState);
            _stateSwitcher.AddState(getReadyState);
            _stateSwitcher.AddState(gameplayState);
            _stateSwitcher.AddState(pauseState);
            _stateSwitcher.AddState(winState);
            _stateSwitcher.AddState(loseState);
            _stateSwitcher.AddState(repairState);
            _stateSwitcher.AddState(finalizeState);
        }

        public void Start() {
            _stateSwitcher.SetState<ConstructState>();
        }
    }
}