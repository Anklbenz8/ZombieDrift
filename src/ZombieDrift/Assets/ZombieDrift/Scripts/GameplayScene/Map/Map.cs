using UnityEditor;
using UnityEngine;
using Unity.AI.Navigation;
using System.Collections.Generic;
using UnityEngine.Experimental.GlobalIllumination;

namespace Gameplay {
    public class Map : MonoBehaviour {
        [SerializeField] private Light _light;
#if UNITY_EDITOR
        [SerializeField] private Transform _zombieSpawnPointsParent;
#endif
        [SerializeField] private NavMeshSurface _navMeshSurface;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform[] _zombieSpawnPointses;

        public Transform[] zombieSpawnPoints => _zombieSpawnPointses;
        public Transform startPoint => _startPoint;
        public NavMeshSurface navMeshSurface => _navMeshSurface;

        public Color lightColor {
            set => _light.color = value;
        }
        
        public Vector3 lightDirection {
            set => _light.transform.localEulerAngles = value;
        }

#if UNITY_EDITOR
        public void UpdateZombiePoints() {
            if (EditorApplication.isPlaying)
                return;

            var zombiePoints = new List<Transform>();
            foreach (Transform child in _zombieSpawnPointsParent)
                zombiePoints.Add(child);

            _zombieSpawnPointses = zombiePoints.ToArray();
        }
#endif
    }
}