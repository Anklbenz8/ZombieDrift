using UnityEngine;

namespace Project {
	[CreateAssetMenu(menuName = "Configs/SoundConfig", fileName = "SoundConfig", order = 0)]
	public class SoundConfig : ScriptableObject {
		[SerializeField] private Sound _soundPrefab;
	
		[SerializeField] private int _poolAmount;

		[SerializeField] private Vector2Int _voiceFrecuencyMinMax;
		[SerializeField] private AudioClip[] _zombieVoicesArray;
		[SerializeField] private AudioClip[] _hitSoundsArray;
		[SerializeField] private AudioClip[] _carChashArray;
		[SerializeField] private AudioClip _carEngineClip;
		[SerializeField] private AudioClip _tyresClip;
		
		public Vector2Int voiceFrequencyMinMax => _voiceFrecuencyMinMax;
		public AudioClip carEngineClip => _carEngineClip;
		public AudioClip tyresClip => _tyresClip;
		public AudioClip[] zombieVoicesArray => _zombieVoicesArray;
		public AudioClip[] hitSoundsArray => _hitSoundsArray;
		public AudioClip[] carCrashArray => _carChashArray;
		public Sound soundPrefab => _soundPrefab;
		public int poolAmount => _poolAmount;
	}
}
