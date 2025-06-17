using UnityEngine;

namespace Gameplay {
	public class FlyingElement : MonoBehaviour {
		public Vector3 position {
			get => transform.position;
			set => transform.position = value;
		}

		public bool isActive {
			set => gameObject.SetActive(value);
			get => gameObject.activeInHierarchy;
		}
	}
}
