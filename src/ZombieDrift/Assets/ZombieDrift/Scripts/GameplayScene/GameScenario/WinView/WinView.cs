using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
	public class WinView : View {
		public event Action ContinueButtonClickedEvent;

		[SerializeField] private TMP_Text _mapClearedCaption;
		[SerializeField] private TMP_Text _mapClearedCaptionShadow;
		[SerializeField] private Button _continueButton;
	
		public string mapText {
			set {
				_mapClearedCaption.text = value;
				_mapClearedCaptionShadow.text = value;
			}
		}

		private void ContinueNotify() =>
				ContinueButtonClickedEvent?.Invoke();
		
		private void OnEnable() =>
				_continueButton.onClick.AddListener(ContinueNotify);

		private void OnDisable() =>
				_continueButton.onClick.RemoveListener(ContinueNotify);
	}
}
