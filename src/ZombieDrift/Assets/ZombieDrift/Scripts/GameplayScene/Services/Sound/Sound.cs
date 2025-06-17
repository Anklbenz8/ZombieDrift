using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Project {
	public class Sound : MonoBehaviour {
		[SerializeField] private AudioSource _audioSource;

		public Vector3 position {
			get => transform.position;
			set => transform.position = value;
		}

		public float loopedPitch {
			get => _audioSource.pitch;
			set => _audioSource.pitch = value;
		}

		public float volume {
			get => _audioSource.volume;
			set => _audioSource.volume = value;
		} 

		public void PlayAndDisableAtPosition(Vector3 pos, AudioClip clip, float pitch = 1) {
			position = pos;
			PlayAndDisable(clip, pitch);
		}

		public async void PlayAndDisable(AudioClip clip, float pitch = 1) {
			var clipLengthMilliseconds = (clip.length / Mathf.Abs(_audioSource.pitch)) * 1000;

			Play(clip, pitch);
			await UniTask.Delay((int)clipLengthMilliseconds);
			StopAndDisable();
		}

		public void PlayLooped(AudioClip clip, float pitch = 1) {
			_audioSource.loop = true;
			Play(clip, pitch);
		}

		public void StopAndDisable() {
			_audioSource.Stop();
			_audioSource.clip = null;
			_audioSource.loop = false;
			_audioSource.pitch = 1;

			gameObject.SetActive(false);
		}

		private void Play(AudioClip clip, float pitch = 1) {
			_audioSource.pitch = pitch;
			_audioSource.clip = clip;
			_audioSource.Play();
		}
	}
}
