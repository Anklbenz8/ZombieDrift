using System;
using TMPro;
using UnityEngine;

public class LoadingScreen : FadeView {
	[SerializeField] private TMP_Text loadingText;
	private void Awake() {
		SetProgress(0);
	}
	public void SetProgress(float progress) {
		//loadingText.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, Mathf.Clamp(-1 + progress, -1, 0));
	}
}
