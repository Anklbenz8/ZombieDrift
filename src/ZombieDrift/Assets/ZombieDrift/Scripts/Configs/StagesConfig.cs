using UnityEngine;

namespace Gameplay {
    [CreateAssetMenu(menuName = "Configs/LevelsConfig", fileName = "LevelsConfig", order = 0)]
    public class StagesConfig : ScriptableObject {

        [SerializeField] private Stage[] _stages;

        public Stage[] stages => _stages;
        
        public int count => _stages.Length;
    }
}