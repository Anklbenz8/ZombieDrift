using Cysharp.Threading.Tasks;
using UnityEngine;
namespace Ads {
	public enum AdsStatus {
		Success,
		ClosedByUser,
	}

	public interface IAdsStrategy {
		public bool isFullscreenAvailable { get; }
		public bool isRewardedAvailable { get; }

		public UniTask<bool> ShowFullscreen();
		public UniTask<bool> ShowRewardVideo();
		
		public void ShowStickyBanner();
	}
}
