using UnityEngine;

namespace Gameplay {

	public class VehicleController : IPauseSensitive {
		public float normalizedVelocity => _car.velocity / _car.maxVelocity;
		public float wheelsAxisHorizontal => _axisHorizontal;
		public Vector3 carPosition => _car.transform.position;

		private readonly IInput _input;
		private Car _car;
		private float _axisHorizontal;

		public VehicleController(IInput input) =>
				_input = input;

		public void SetCar(Car car) =>
				_car = car;

		public void Start() {
			_input.HorizontalAxisChangedEvent += OnTurn;
			_car.isRunning = true;
		}

		public void Stop() {
			_input.HorizontalAxisChangedEvent -= OnTurn;
			_car.isRunning = false;
		}

		private void OnTurn(float axisHorizontal) {
			_axisHorizontal = axisHorizontal;
			_car.turnHorizontalAxis = _axisHorizontal;
		}
		public void SetPause(bool isPaused) {
			_car.SetPause(isPaused);
		}
	}
}
