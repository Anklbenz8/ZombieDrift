using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Garage {
	public class GarageView : MonoBehaviour {
		private const float BUY_NOT_INTERACTABLE_ALPHA = 0.4f;

		[SerializeField] private Button _leftButton;
		[SerializeField] private Button _rightButton;
		[SerializeField] private Button _buyButton;
		[SerializeField] private Button _selectButton;
		[SerializeField] private Button _watchVideoButton;
		[SerializeField] private Button _backButton;
		
		[SerializeField] private TMP_Text _carPriceText;
		[SerializeField] private TMP_Text _moneyCount;
		[SerializeField] private TMP_Text _comboMultiplier;
		//[SerializeField] private TMP_Text _comboDelay;
		
		[SerializeField] private GameObject _buyControl, _watchControl, _selectControl, _selectedControl, _lock;
		[SerializeField] private CanvasGroup _buyButtonCanvasGroup;

		public Button leftButton => _leftButton;
		public Button rightButton => _rightButton;
		public Button buyButton => _buyButton;
		public Button selectButton => _selectButton;
		public Button watchVideoButton => _watchVideoButton;
		public Button backButton => _backButton;
		public TMP_Text carPriceText => _carPriceText;
		public TMP_Text moneyCount => _moneyCount;
		public TMP_Text comboMultiplier => _comboMultiplier;
	//	public TMP_Text comboDelay => _comboDelay;
		
		public bool isLockVisible {
			set => _lock.SetActive(value);
		}

		public bool isBuyControlActive {
			set => _buyControl.gameObject.SetActive(value);
		}
		public bool isBuyControlInteractable {
			set {
				_buyButtonCanvasGroup.alpha = value ? 1 : BUY_NOT_INTERACTABLE_ALPHA;
				_buyButtonCanvasGroup.interactable = value;
			}
		}

		public bool isComboMultiplierEnabled {
			set => _comboMultiplier.gameObject.SetActive(value);
		}
		
		public bool isSelectControlEnabled {
			set => _selectControl.gameObject.SetActive(value);
		}

		public bool isWatchControlActive {
			set => _watchControl.gameObject.SetActive(value);
		}

		public bool isSelectedControlActive {
			set => _selectedControl.gameObject.SetActive(value);
		}
	}
}
