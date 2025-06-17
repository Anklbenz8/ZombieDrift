using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
namespace Gameplay {
	public class Zombie : MonoBehaviour, IDamageable {

		private const int LOW_PRIORITY_VALUE = 10;
		private const int HIGH_PRIORITY_VALUE = 100;

		[SerializeField] private Animator animator;
		[SerializeField] private Collider triggerCollider;
		[SerializeField] private Ragdoll ragdoll;

		[SerializeField] private NavMeshAgent _navMeshAgent;
		[SerializeField] private ParticleSystem _groundCircleParticles;

		/*[SerializeField] */
		private Vector2 _speedInterval /*= new(3, 8)*/;

		public Vector2 speedInterval {
			set {
				_speedInterval = value;
				_navMeshAgent.speed = Random.Range(_speedInterval.x, _speedInterval.y);
			}
		}

		public Vector3 position => transform.position;

		public Vector3 destination {
			set => _navMeshAgent.SetDestination(value);
		}

		public bool isNavEnabled {
			set => _navMeshAgent.enabled = value;
		}

		public bool isRunning {
			set => _navMeshAgent.isStopped = !value;
		}

		private ZombieAnimator _zombieAnimator;

		public void Awake() {
			_zombieAnimator = new ZombieAnimator(transform, animator);
			_navMeshAgent.updateRotation = false;
			_navMeshAgent.avoidancePriority = Random.Range(LOW_PRIORITY_VALUE, HIGH_PRIORITY_VALUE);
			_navMeshAgent.speed = Random.Range(_speedInterval.x, _speedInterval.y);

			ragdoll.Initialize();
			ragdoll.isEnabled = false;
		}

		public void Deactivate() {
			_zombieAnimator.isEnabled = false;
			_groundCircleParticles.gameObject.SetActive(false);
			isNavEnabled = false;
			ragdoll.isEnabled = true;
			triggerCollider.enabled = false;
		}

		private void FixedUpdate() =>
				_zombieAnimator.PlayMove(_navMeshAgent.desiredVelocity);
	}
}
