using UnityEngine;

namespace Project {
	[CreateAssetMenu(menuName = "Configs/UiSoundConfig", fileName = "UiSoundConfig", order = 0)]
	public class UiSoundConfig : ScriptableObject {
		[SerializeField] private Sound _soundPrefab;
		[SerializeField] private int _poolAmount;

		[SerializeField] private AudioClip _buttonAudioClip;
		[SerializeField] private AudioClip _winAudioClip;
		[SerializeField] private AudioClip _loseAudioClip;
		[SerializeField] private AudioClip _carSelectAudioClip;
		[SerializeField] private AudioClip _carBuyAudioClip;
		[SerializeField] private AudioClip _coinAudioClip;
		[SerializeField] private AudioClip _repairAudioClip;

		public AudioClip buttonAudioClip => _buttonAudioClip;
		public AudioClip coinAudioClip => _coinAudioClip;
		public AudioClip winAudioClip => _winAudioClip;
		public AudioClip loseAudioClip => _loseAudioClip;
		public AudioClip carSelectAudioClip => _carSelectAudioClip;
		public AudioClip carBuyAudioClip => _carBuyAudioClip;
		public AudioClip repairAudioClip => _repairAudioClip;
		public Sound soundPrefab => _soundPrefab;
		public int poolAmount => _poolAmount;
	}
}
