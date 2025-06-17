using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gameplay {
    public class BotNavigation {
        public Transform target { get; set; }
    
        private readonly float _positionRefreshRate;
        private readonly float _reactDistance;
        
        private IReadOnlyCollection<Zombie> _zombies;
        private float _totalSeconds;
        private bool _isRunning;

        public BotNavigation(ZombiesConfig zombiesConfig) {
            _positionRefreshRate = zombiesConfig.navigationRefreshTargetRate;
            _reactDistance = zombiesConfig.navigationReactDistance;
        }

        public void Initialize(IReadOnlyCollection<Zombie> zombies, Transform targetTransform) {
            //_zombies = zombies.ToList();
            _zombies = zombies;
            target = targetTransform;
        }

        public void Start() {
            _isRunning = true;
            SetZombiesRunning(true);
        }

        public void Stop() {
            _isRunning = false;
            SetZombiesRunning(false);
        }

        public void Tick() {
            if (!_isRunning) return;
            _totalSeconds += Time.deltaTime;
            if (_totalSeconds < _positionRefreshRate)
                return;
            _totalSeconds = 0;
            RefreshPosition();
        }

        /*public void RemoveKilledZombie(Zombie killedZombie) {
            _zombies.Remove(killedZombie);
        }*/

        private void SetZombiesRunning(bool isRun) {
            foreach (var zombie in _zombies)
                zombie.isRunning = isRun;
        }

        private void RefreshPosition() {
            foreach (var zombie in _zombies) {
                var d = Vector3.Distance(target.position, zombie.position);
                if (d < _reactDistance)
                    zombie.destination = target.position;
            }
        }
    }
}