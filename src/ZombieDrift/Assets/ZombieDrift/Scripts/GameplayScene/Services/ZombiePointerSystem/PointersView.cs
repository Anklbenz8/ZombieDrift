using UnityEngine;

namespace Gameplay {
    public class PointersView : View {
        [SerializeField] private Transform _parent;
        [SerializeField] private Pointer _prefab;

        public Transform parent => _parent;
        public Pointer prefab => _prefab;
    }
}