using Cysharp.Threading.Tasks;
using GamePush;
using UnityEngine;
namespace Ads {
	public class GamePushStrategy : IAdsStrategy {
		private UniTaskCompletionSource<bool> _fullscreenSource;
		public bool isFullscreenAvailable => GP_Ads.IsFullscreenAvailable();
		public bool isRewardedAvailable => GP_Ads.IsRewardedAvailable();

		public async UniTask<bool> ShowFullscreen() {
			_fullscreenSource = new UniTaskCompletionSource<bool>();
			GP_Ads.ShowFullscreen(OnFullscreenStart, OnFullscreenClose);
			return await _fullscreenSource.Task;
		}

		public async UniTask<bool> ShowRewardVideo() {
			_fullscreenSource = new UniTaskCompletionSource<bool>();
		
			GP_Ads.ShowRewarded(string.Empty, null, OnRewardedStart, OnRewardedClose);
			return await _fullscreenSource.Task;
		}

		public void ShowStickyBanner() {
			GP_Ads.ShowSticky();
		}

		private void OnRewardedClose(bool success) {
			Debug.Log($"Reward success {success}");
			_fullscreenSource.TrySetResult(success);
		}
		private void OnRewardedStart() {
	
			Debug.Log("ON REWARDED START");
		}
		private void OnFullscreenStart() {
			Debug.Log("ON FULLSCREEN START");
		}
		private void OnFullscreenClose(bool success) {
			_fullscreenSource.TrySetResult(true);
		}
	}
}
