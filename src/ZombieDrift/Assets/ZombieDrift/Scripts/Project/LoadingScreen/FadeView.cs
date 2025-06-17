using Cysharp.Threading.Tasks;
using UnityEngine;
public class FadeView : MonoBehaviour {
	private const int DEFAULT_FADE_TIME = 20;
	[SerializeField] private CanvasGroup canvasGroup;

	private float alpha {
		get => canvasGroup.alpha;
		set => canvasGroup.alpha = value;
	}

	private bool isEnabled {
		set => gameObject.SetActive(value);
		get => gameObject.activeInHierarchy;
	}

	public async void Appear(int timeMilliseconds = DEFAULT_FADE_TIME) =>
		await AppearAsync(timeMilliseconds);

	public async UniTask AppearAsync(int timeMilliseconds = DEFAULT_FADE_TIME) {
		if (timeMilliseconds == 0)
			timeMilliseconds = 1;
		alpha = 0;
		isEnabled = true;
		var iteration = 1 / (float)timeMilliseconds;

		for (float i = 0; i < timeMilliseconds; i++) {
			alpha += iteration;
			await UniTask.Delay(1);
		}
	}

	public async void Disappear(int timeMilliseconds = DEFAULT_FADE_TIME) {
		if (timeMilliseconds == 0)
			timeMilliseconds = 1;
		alpha = 1;

		var iteration = 1 / (float)timeMilliseconds;
		for (float i = 0; i < timeMilliseconds; i++) {
			alpha -= iteration;
			await UniTask.Delay(1);
		}
		isEnabled = false;
	}
	
}
