using UnityEngine;

namespace Gameplay {
	[CreateAssetMenu(menuName = "Configs/TextHintConfig", fileName = "TextHintConfig", order = 0)]
	public class TextHintConfig : ScriptableObject {
		[SerializeField] private TextHint _textHintPrefab;
		[SerializeField] private int _poolSize = 3;
		[SerializeField] private int _showTimeMilliseconds= 1000;
		[SerializeField] private Vector3 _offset = new(0, 2, 0);
		public TextHint textHintPrefab => _textHintPrefab;
		public int poolSize => _poolSize;
		public Vector3 offset => _offset;
		public int showTimeMillisecondsMilliseconds => _showTimeMilliseconds;
	}
}
