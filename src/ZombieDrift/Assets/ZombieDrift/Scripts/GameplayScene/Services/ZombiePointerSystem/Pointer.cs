using UnityEngine;

namespace Gameplay {
    public class Pointer : MonoBehaviour {
        public bool isVisible {
            set => gameObject.SetActive(value);
        }

        public Vector3 position {
            set => transform.position = value;
            get => transform.position;
        }
    }
}