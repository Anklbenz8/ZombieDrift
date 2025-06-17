using System.Collections.Generic;
using Project;
using UnityEngine;

namespace Gameplay {
	public class TextHintSystem {
		private const string PARTICLES_PARENT_NAME = "HintParent";

		private readonly PoolObjects<TextHint> _hintPool;
		private readonly int _showTimeMilliseconds;
		private readonly Vector3 _offset;

		private Camera _camera;
	
		public TextHintSystem(TextHintConfig config) {
			var particlesParent = new GameObject(PARTICLES_PARENT_NAME);
			_showTimeMilliseconds = config.showTimeMillisecondsMilliseconds;
			_hintPool = new PoolObjects<TextHint>(config.textHintPrefab, config.poolSize, true, particlesParent.transform);
			_offset = config.offset;
		}

		public void Initialize(Camera mainCamera) =>
				_camera = mainCamera;

		public void ShowHint(Vector3 position, string hintText) {
			var hint = _hintPool.GetFreeElement();
			hint.position = position + _offset;

			hint.Show(hintText, _showTimeMilliseconds);
			var lookPos = hint.position - _camera.transform.position;
			hint.transform.LookAt(lookPos);
		}
	}
}
