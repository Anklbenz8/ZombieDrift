using System;
using UnityEngine;

namespace Gameplay {

    public class GameProcess {
	    private readonly BotNavigation _botNavigation;
	    public event Action<Vector3> ObstacleHitEvent;
        public event Action<Zombie> ZombieHitEvent;
        public event Action AllEnemiesDestroyedEvent;

        //private List<Zombie> _damageableList;
        private Car _car;
        private ZombieStorage _zombieStorage;
        
        public void Initialize(Car car, ZombieStorage zombies/*Zombie[] zombies*/) {
          //  _damageableList = new List<Zombie>(zombies);
           _zombieStorage = zombies;
            SetCar(car);
        }

        public void SetCar(Car car) {
            _car = car;
            _car.HitDamageableEvent += OnDamageableHit;
            _car.CarDestroyedEvent += OnObstacleHit;
        }

        public void Finish() {
            _car.HitDamageableEvent -= OnDamageableHit;
            _car.CarDestroyedEvent -= OnObstacleHit;
            _car = null;
            //_damageableList = null;
        }

        private void OnObstacleHit(Vector3 obj) {
            ObstacleHitEvent?.Invoke(obj);
        }

        private void OnDamageableHit(Zombie damageable) {
            //_damageableList.Remove(damageable);
            _zombieStorage.Deactivate(damageable);
            ZombieHitEvent?.Invoke(damageable);

            //if (_damageableList.Count == 0)
            if (_zombieStorage.Count == 0)
                AllEnemiesDestroyedEvent?.Invoke();
        }
    }
}