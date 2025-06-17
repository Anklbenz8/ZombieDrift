using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Project {
	public class ScenesLoader {
		private LoadingScreen _loadingScreen;

		public ScenesLoader(RootCanvas rootCanvas) =>
				_loadingScreen = rootCanvas.loadingScreen;

		public void SwitchToShopScene() =>
				SwitchSceneWithAnimation(SceneNames.GARAGE);

		public void SwitchToGameplayScene() =>
				SwitchSceneWithAnimation(SceneNames.GAMEPLAY);

		private async void SwitchSceneWithAnimation(string sceneName) {
			await _loadingScreen.AppearAsync();

			//	await SwitchTo(SceneNames.BOOT);
			await SwitchTo(sceneName);

			_loadingScreen.Disappear();
		}

		private async UniTask SwitchTo(string sceneName) {
			var switchSceneAsyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
			while (!switchSceneAsyncOperation.isDone) {
				_loadingScreen.SetProgress(switchSceneAsyncOperation.progress);
				await UniTask.Yield();
			}
		}
	}
}
