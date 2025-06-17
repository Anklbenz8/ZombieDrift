using UnityEngine;

namespace Gameplay {
    [CreateAssetMenu(fileName = "ParticlesConfig", menuName = "Configs/ParticlesConfig")]
    public class ParticlesConfig : ScriptableObject {
        [SerializeField] private ParticleSystem _blood;
        [SerializeField] private ParticleSystem _carHit;


        public ParticleSystem blood => _blood;
        public ParticleSystem carHit => _carHit;
    }
}