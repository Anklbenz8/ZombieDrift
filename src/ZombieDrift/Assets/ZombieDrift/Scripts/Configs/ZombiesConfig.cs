using UnityEngine;
namespace Gameplay {

	[CreateAssetMenu(menuName = "Configs/ZombiesConfig", fileName = "ZombiesConfig", order = 0)]
	public class ZombiesConfig : ScriptableObject {
		[SerializeField] private Vector2 _speedIntervalMinMax;
		[Header("Navigation")]
		[SerializeField] private float _refreshTargetRate = 0.2f;
		[SerializeField] private float _reactDistance = 8f;
		[Header("Prefabs")]
		[SerializeField] private Zombie[] _zombies;

		public float navigationRefreshTargetRate => _refreshTargetRate;
		public float navigationReactDistance => _reactDistance;
		public Vector2 speedIntervalMinMax => _speedIntervalMinMax;
		public Zombie[] zombies => _zombies;
		public int count => zombies.Length;
	}
}
