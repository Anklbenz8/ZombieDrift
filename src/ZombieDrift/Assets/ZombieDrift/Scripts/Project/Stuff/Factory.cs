using UnityEngine;
using Zenject;

namespace Project {
    public class Factory {
        private readonly DiContainer _diContainer;

        public Factory(DiContainer diContainer) {
            _diContainer = diContainer;
        }

        public T CreateAndBind<T>(string path, Transform parent = null, Vector3 position = default, Quaternion rotation = default) where T : Object =>
            _diContainer.InstantiatePrefabResourceForComponent<T>(path, position, rotation, parent);

        public T CreateAndBind<T>(T prefab, Transform parent = null, Vector3 position = default, Quaternion rotation = default) where T : Object =>
            _diContainer.InstantiatePrefabForComponent<T>(prefab, position, rotation, parent);


        public T Create<T>(string path, Transform parent = null, Vector3 position = default, Quaternion rotation = default) where T : Object {
            var prefab = Resources.Load<T>(path);
            return Create<T>(prefab, parent, position, rotation);
        }

        public T Create<T>(T prefab, Transform parent = null, Vector3 position = default, Quaternion rotation = default) where T : Object =>
            Object.Instantiate(prefab, position, rotation, parent);
    }
}