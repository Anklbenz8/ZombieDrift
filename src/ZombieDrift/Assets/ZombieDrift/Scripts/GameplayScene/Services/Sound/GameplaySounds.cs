using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Project {
	public class GameplaySounds {
		private const string POOL_SOUNDS_PARENT_NAME = "SoundsParent";
		private const float MIN_PINCH = 0.9f;
		private const float MAX_PINCH = 1.1f;
		private const float DEFAULT_VOLUME = 1f;
		public bool isMute {
			get => AudioListener.volume == 0; // !_audioListener;//AudioListener.pause;
			set => AudioListener.volume = value 
					? 0 
					: DEFAULT_VOLUME; //_audioListener.enabled = !value;
		}

		private readonly SoundConfig _soundConfig;
		private PoolObjects<Sound> _poolOfSounds;
		private Transform _soundsParent;
		private Sound _engineSound, _tyresSound;
		private float randomPinchValue => Random.Range(MIN_PINCH, MAX_PINCH);

		private bool _zombieVoicesPlaying;
		private AudioListener _audioListener;

		public GameplaySounds(SoundConfig soundConfig) {
			_soundConfig = soundConfig;
		}

		public void Initialize(AudioListener audioListener) {
			_audioListener = audioListener;
			_soundsParent = new GameObject(POOL_SOUNDS_PARENT_NAME).transform;
			_poolOfSounds = new PoolObjects<Sound>(_soundConfig.soundPrefab, _soundConfig.poolAmount, canExpand: true, _soundsParent);
		}

		public void PlayZombieHitSoundAtPosition(Vector3 position) {
			var hitSoundsArray = _soundConfig.hitSoundsArray;
			PlayRandomSoundAtPosition(position, hitSoundsArray, randomPinchValue);
		}

		public void PlayCarCrashSoundAtPosition(Vector3 position) {
			var soundsArray = _soundConfig.carCrashArray;
			PlayRandomSoundAtPosition(position, soundsArray, 1);
		}

		public async void StartZombieVoices(IReadOnlyCollection<Zombie> zombies) {
			_zombieVoicesPlaying = true;

			var soundsArray = _soundConfig.zombieVoicesArray;
			var frequency = _soundConfig.voiceFrequencyMinMax;

			while (_zombieVoicesPlaying) {
				var activeZombieCount = zombies.Count;

				if (activeZombieCount == 0) {
					StopZombieVoices();
					return;
				}

				int randomIndex = Random.Range(0, activeZombieCount);
				var randomZombie = zombies.ElementAt(randomIndex);
				var randomZombiePosition = randomZombie.transform.position;

				PlayRandomSoundAtPosition(randomZombiePosition, soundsArray, 1);
				var randomDelay = Random.Range(frequency.x, frequency.y);
				await UniTask.Delay(randomDelay);
			}
		}
		public void StopZombieVoices() {
			_zombieVoicesPlaying = false;
		}

		public void StartCarSounds() {
			_engineSound = _poolOfSounds.GetFreeElement();
			_engineSound.PlayLooped(_soundConfig.carEngineClip, 0);

			_tyresSound = _poolOfSounds.GetFreeElement();
			_tyresSound.PlayLooped(_soundConfig.tyresClip, 1);
		}

		public void UpdateCarSounds(Vector3 position, float normalizedSpeed, float wheelAxis) {
			_engineSound.position = position;
			_engineSound.volume = /*isMute
					? 0
					:*/ 1;

			_engineSound.loopedPitch = normalizedSpeed;

			_tyresSound.position = position;
			//Half tyres volume from engine, another half from wheelAxis
			_tyresSound.volume = /*isMute
					? 0
					:*/ (normalizedSpeed + wheelAxis) * 0.5f;
		}

		public void StopCarSounds() {
			_tyresSound.StopAndDisable();
			_tyresSound = null;

			_engineSound.StopAndDisable();
			_engineSound = null;
		}

		private void PlayRandomSoundAtPosition(Vector3 position, AudioClip[] soundsArray, float pitch) {
			if (soundsArray.Length == 0 /*|| isMute*/)
				return;

			int randomIndex = Random.Range(0, soundsArray.Length);
			var audioClip = soundsArray[randomIndex];

			var sound = _poolOfSounds.GetFreeElement();

			sound.PlayAndDisableAtPosition(position, audioClip, pitch);
		}
	}
}
