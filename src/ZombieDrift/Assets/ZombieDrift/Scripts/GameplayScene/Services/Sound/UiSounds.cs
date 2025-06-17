using UnityEngine;

namespace Project {
	public class UiSounds {
		private const string POOL_SOUNDS_PARENT_NAME = "SoundsParent";
		private const float MIN_PINCH = 1f;
		private const float MAX_PINCH = 1.2f;
		public bool isMute { get; set; }

		private readonly UiSoundConfig _soundConfig;
		private PoolObjects<Sound> _poolOfSounds;
		private Transform _soundsParent;

		public UiSounds(UiSoundConfig soundConfig) =>
				_soundConfig = soundConfig;
		public void Initialize(Transform poolParent) {
			_soundsParent = poolParent;
			CreatePool();
		}
		
		public void Initialize() {
			_soundsParent = new GameObject(POOL_SOUNDS_PARENT_NAME).transform;
			CreatePool();
		}

		private void CreatePool() =>
				_poolOfSounds = new PoolObjects<Sound>(_soundConfig.soundPrefab, _soundConfig.poolAmount, canExpand: true, _soundsParent);

		public void PlayClickSound() {
			var randomPitch = Random.Range(MIN_PINCH, MAX_PINCH);
			Play(_soundConfig.buttonAudioClip, randomPitch);
		}

		public void PlayCoinSound() {
			var randomPitch = Random.Range(MIN_PINCH, MAX_PINCH);
			Play(_soundConfig.coinAudioClip, randomPitch);
		}

		public void PlayBuySound() =>
				Play(_soundConfig.carBuyAudioClip);

		public void PlayLoseSound() =>
				Play(_soundConfig.loseAudioClip);

		public void PlayWinSound() =>
				Play(_soundConfig.winAudioClip);

		public void PlayRepairSound() =>
				Play(_soundConfig.repairAudioClip);

		private void Play(AudioClip clip, float pinch = 1) {
			if (isMute)
				return;

			var sound = _poolOfSounds.GetFreeElement();
			sound.PlayAndDisable(clip, pinch);
		}
		
	}
}
