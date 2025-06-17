using System.Collections.Generic;
using System.Linq;
using Project;
using UnityEngine;
using Plane = UnityEngine.Plane;

namespace Gameplay {
	public class EnemyPointerSystem {
		private readonly Factory _factory;
		private PointersView _view;
		private Dictionary<Zombie, Pointer> _targets = new();
		private Transform _player;
		private Pointer _pointerPrefab;
		private Transform _pointersParent;
		private Camera _camera;

		private Vector2 _screenCenter;
		public bool enabled {
			set {
				if (value == _enabled)
					return;
				_enabled = value;
				_view.isActive = value;
			}
		}

		public Transform player {
			get => _player;
			set => _player = value;
		}

		private bool _enabled;

		public EnemyPointerSystem(Factory factory) {
			_factory = factory;
		}

		public void Initialize(PointersView pointersParent, Camera camera) {
			_view = pointersParent;
			_pointersParent = _view.parent;
			_pointerPrefab = _view.prefab;
			_camera = camera;
		}

		public void SetNewData(Zombie[] targets, Transform player) {
			Clear();
			_targets = new();
			_player = player;
			Add(targets);
		}


		public void Add(Zombie[] targets) {
			foreach (var target in targets)
				Add(target);
		}

		public void Add(Zombie target) {
			var pointer = _factory.Create(_pointerPrefab, _pointersParent);
			_targets.Add(target, pointer);
		}

		public void Remove(Zombie target) {
			if (!_targets.ContainsKey(target)) return;
			_targets.Remove(target, out var pointer);
			Object.Destroy(pointer.gameObject);
		}

		public void Clear() {
			var keys = _targets.Keys.ToArray();
			foreach (var key in keys)
				Remove(key);
		}

		public void Tick() {
			if (!_enabled) return;
			Plane[] cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);
			foreach (var target in _targets) {
				SetPointerPosition(target, cameraFrustumPlanes);
			}
		}

		private void SetPointerPosition(KeyValuePair<Zombie, Pointer> targetPointerPair, Plane[] cameraFrustumPlanes) {
			var target = targetPointerPair.Key;
			var pointer = targetPointerPair.Value;

			var playerPosition = _player.position;

			var vectorFromPlayerToTarget = target.position - playerPosition;
			var rayFromPlayerToTarget = new Ray(playerPosition, vectorFromPlayerToTarget);

			float distanceToNearPlane = Mathf.Infinity;
			for (var i = 0; i < 4; i++) {
				if (!cameraFrustumPlanes[i].Raycast(rayFromPlayerToTarget, out var distance))
					continue;

				if (distance < distanceToNearPlane)
					distanceToNearPlane = distance;
			}

			var isPointerMustBeShown = vectorFromPlayerToTarget.magnitude > distanceToNearPlane;
			pointer.isVisible = isPointerMustBeShown;
			if (!isPointerMustBeShown)
				return;
			var pointerWorldPosition = rayFromPlayerToTarget.GetPoint(distanceToNearPlane);
			var pointerScreenPoint = _camera.WorldToScreenPoint(pointerWorldPosition);
			var playerScreenPoint = _camera.WorldToScreenPoint(playerPosition);

			pointerScreenPoint.z = 0;
			pointer.position = pointerScreenPoint;

			int angle = (int)Vector2.SignedAngle(playerScreenPoint - pointerScreenPoint, new Vector3((float)Screen.width / 2, (float)Screen.height / 2) - new Vector3((float)Screen.width / 2, 0));
			pointer.transform.rotation = Quaternion.AngleAxis(-angle, Vector3.forward);
		}
	}
}
