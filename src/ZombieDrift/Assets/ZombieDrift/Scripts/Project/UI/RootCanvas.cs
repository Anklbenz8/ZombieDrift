using UnityEngine;

public class RootCanvas : MonoBehaviour {
	[SerializeField] private LoadingScreen _loadingScreen;
	public LoadingScreen loadingScreen => _loadingScreen;
	private void Awake() =>
			HideLoadingScreen();
	public void HideLoadingScreen() =>
			_loadingScreen.gameObject.SetActive(false);
}
