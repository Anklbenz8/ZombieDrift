using UnityEngine;

namespace Gameplay {

	[CreateAssetMenu(menuName = "Configs/CarsConfig", fileName = "CarsConfig", order = 0)]
	public class CarsConfig : ScriptableObject {
		[SerializeField] private LayerMask _lockedLayerMask, _purchasedLayerMask;
		[SerializeField] private CarData[] _cars;
		public LayerMask purchasedLayerMask => _purchasedLayerMask;
		public LayerMask lockedLayerMask => _lockedLayerMask;
		public CarData[] cars => _cars;
	}
}
