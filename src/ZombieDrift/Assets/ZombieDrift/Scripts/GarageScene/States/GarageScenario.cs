using Project;
namespace Garage {
	public class GarageScenario {
		private readonly StateSwitcher _stateSwitcher;

		public GarageScenario(
				StateSwitcher stateSwitcher,
				InitializeState initializeState,
				GarageState garageState) {
			_stateSwitcher = stateSwitcher;
			_stateSwitcher.AddState(initializeState);
			_stateSwitcher.AddState(garageState);
		}

		public void Start() =>
				_stateSwitcher.SetState<InitializeState>();
	}
}
