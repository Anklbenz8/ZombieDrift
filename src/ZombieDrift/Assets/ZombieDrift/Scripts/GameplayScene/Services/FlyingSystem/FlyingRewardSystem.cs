using System;
using UnityEngine;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Project;
using Zenject;
using Random = UnityEngine.Random;

namespace Gameplay {
    public class FlyingRewardSystem : IFixedTickable, IPauseSensitive {
        public event Action CollectedEvent, AllCollectedEvent;

        private readonly FlyingRewardConfig _config;
        private readonly List<FlyingElement> _flyingList = new();
        private readonly PoolObjects<FlyingElement> _flyingItemsPool;
        private Camera _mainCamera;
        private Transform _destinationTransform;
        private bool _isPaused;
        
        public FlyingRewardSystem(FlyingRewardConfig config) {
            _config = config;
            var parent = new GameObject(_config.itemsParentName).transform;
            _flyingItemsPool = new PoolObjects<FlyingElement>(_config.prefab, _config.poolAmount, true, parent);
        }

        public void Initialize(Camera mainCamera, Transform destination) {
            _mainCamera = mainCamera;
            _destinationTransform = destination;
        }

        public void SpawnInSphere(Vector3 sphereCenter, int count=1) {
            for (var i = 0; i < count; i++) {
                var randomPointInSphere = Random.insideUnitSphere * _config.radius + sphereCenter + _config.offsetInSphere;
                CreateWithDelays(randomPointInSphere);
            }
        }

        public void SpawnInCircle(Vector2 circleCenter, int count) {
            for (var i = 0; i < count; i++) {
                var randomPointInCircle = Random.insideUnitCircle * _config.radius + circleCenter + _config.offsetInCircle;
                CreateWithDelays(randomPointInCircle);
            }
        }

        private async void CreateWithDelays(Vector3 position) {
            await UniTask.Delay(_config.delayBeforeAppear);
            var coin = _flyingItemsPool.GetFreeElement();
            coin.position = position;

            AddToFlyingListWithDelay(coin);
        }

        private async void AddToFlyingListWithDelay(FlyingElement coinTransform) {
            await UniTask.Delay(_config.delayBeforeStartMoving);
            _flyingList.Add(coinTransform);
        }

        public void FixedTick() {
	        if(_isPaused)
		        return;
	        
	        if (_flyingList.Count <= 0)
                return;
            Fly();
        }

        private void Fly() {
            var destinationPosition = GetDestinationPosition();
            for (var i = _flyingList.Count - 1; i >= 0; i--) {
                _flyingList[i].position = Vector3.MoveTowards(_flyingList[i].position, destinationPosition, _config.coinSpeed);

                var flyingArrived = _flyingList[i].position == destinationPosition;
                if (!flyingArrived)
                    continue;

                _flyingList[i].isActive = false;
                _flyingList.RemoveAt(i);

                CollectedEvent?.Invoke();

                if (_flyingList.Count == 0)
                    AllCollectedEvent?.Invoke();
            }
        }

        private Vector3 GetDestinationPosition() {
            var destinationPosition = _destinationTransform.position;
            return _mainCamera.ScreenToWorldPoint(new Vector3(destinationPosition.x, destinationPosition.y, _mainCamera.nearClipPlane));
        }
        public void SetPause(bool isPaused) {
	        _isPaused = isPaused;
        }
    }
}