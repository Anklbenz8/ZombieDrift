using System;
using UnityEngine;

namespace Gameplay {
	public class Car : MonoBehaviour, IPauseSensitive {
		public event Action<Zombie> HitDamageableEvent;
		public event Action<Vector3> CarDestroyedEvent;

		[Header("DriveSettings")]
		[SerializeField] private float _maxSpeed = 15;

		[SerializeField] private float _moveSpeed = 40;
		[SerializeField] private float _steerAngle = 0.25f;
		[SerializeField] private float _mass = 1000;
		[SerializeField] private float _drag = 0;
		[SerializeField] private float _angularDrag = 0.02f;
		[SerializeField] private Vector3 _centerOfMass = new(0, -0.2f, 0);

		[Header("Components")]
		[SerializeField] private RigidbodyMotor _motor;

		[SerializeField] private GameObject _carMesh;
		[SerializeField] private WheelBehaviour _wheelBehaviour;
		[SerializeField] private CarParticles _carParticles;
		[SerializeField] private WheelTrails _wheelTrails;
		public Rigidbody body => _motor.body;
		public GameObject mesh => _carMesh;
		public float velocity => _motor.rigidbodyVelocity;
		public float maxVelocity => _maxSpeed;
		private float _horizontalAxis;

		public bool isRunning {
			get => _isRunning;
			set {
				_isRunning = value;
				_carParticles.isWheelSmokeEnabled = isRunning;
				_wheelTrails.isTrailsEmitting = isRunning;
			}
		}

		public float turnHorizontalAxis {
			get => _horizontalAxis;
			set {
				_horizontalAxis = value;
				_wheelBehaviour.wheelTurn = _horizontalAxis;
				_motor.steerTurn = _horizontalAxis;
			}
		}

		private bool _isRunning;

		public void Initialize() {
			_motor.maxSpeed = _maxSpeed;
			_motor.acceleration = _moveSpeed;
			_motor.steerAngle = _steerAngle;
			_motor.centerOfMass = _centerOfMass;
			_motor.drag = _drag;
			_motor.angularDrag = _angularDrag;
			_motor.mass = _mass;
		}

		public void Crash() {
			_carParticles.isCrashSmokeEnabled = true;
		}

		private void FixedUpdate() {
			if (!isRunning)
				return;

			_motor.FixedTick();
			_wheelBehaviour.FixedTick();
		}

		private void OnTriggerEnter(Collider other) {
			if (!_isRunning)
				return;
			if (!other.TryGetComponent<Zombie>(out var damageable))
				return;

			HitDamageableEvent?.Invoke(damageable);
		}

		private void OnCollisionEnter(Collision other) {
			if (!other.gameObject.TryGetComponent<IObstacle>(out _))
				return;

			CarDestroyedEvent?.Invoke(other.contacts[0].point);
		}
		public void SetPause(bool isPaused) {
			_isRunning = !isPaused;
			_motor.SetPause(isPaused);
			_carParticles.SetPause(isPaused);
		}
	}
}
