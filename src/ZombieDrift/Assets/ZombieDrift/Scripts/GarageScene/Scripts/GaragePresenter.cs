using System;
using Project;
using UnityEngine.Localization;

namespace Garage {
	public enum GarageItemState {
		Locked = 0,
		NotEnoughMoney = 1,
		Purchased = 2,
		Selected = 3
	}

	public class GaragePresenter {
		private const string LOCALIZE_TABLE = "StringsTable";
		private const string BUY_LOCAL_KEY = "buyKey";
		private const string COMBO_MULTIPLIER_LOCAL_KEY = "comboMultiplierKey";

		public event Action BuyEvent, ChooseEvent, WatchEvent, PreviousClickedEvent, NextClickedEvent, BackEvent;
		private readonly UiSounds _uiSounds;
		private readonly LocalizedString _buyLocalizedString;
		private readonly LocalizedString _comboMultiplierLocalizedString;

		public GaragePresenter(UiSounds uiSounds) {
			_uiSounds = uiSounds;
			_buyLocalizedString = new LocalizedString(LOCALIZE_TABLE, BUY_LOCAL_KEY);
			_comboMultiplierLocalizedString = new LocalizedString(LOCALIZE_TABLE, COMBO_MULTIPLIER_LOCAL_KEY);
		}

		public int carPrice {
			set => _view.carPriceText.text = _buyLocalizedString.GetLocalizedString(value);
		}

		public int moneyCount {
			set => _view.moneyCount.text = $"{value}";
		}

		public float comboMultiplier {
			set => _view.comboMultiplier.text = _comboMultiplierLocalizedString.GetLocalizedString(value);
		}

		public GarageItemState state {
			set {
				switch (value) {
					case GarageItemState.Locked:
						_view.isBuyControlActive = true;
						_view.isBuyControlInteractable = true;
						_view.isWatchControlActive = true;
						_view.isSelectControlEnabled = false;
						_view.isSelectedControlActive = false;
						_view.isLockVisible = true;

						break;
					case GarageItemState.NotEnoughMoney:
						_view.isBuyControlActive = true;
						_view.isBuyControlInteractable = false;
						_view.isWatchControlActive = true;
						_view.isSelectControlEnabled = false;
						_view.isSelectedControlActive = false;
						_view.isLockVisible = true;

						break;
					case GarageItemState.Purchased:
						_view.isBuyControlActive = false;
						_view.isWatchControlActive = false;
						_view.isSelectedControlActive = false;
						_view.isSelectControlEnabled = true;
						_view.isLockVisible = false;

						break;
					case GarageItemState.Selected:
						_view.isBuyControlActive = false;
						_view.isSelectControlEnabled = false;
						_view.isWatchControlActive = false;
						_view.isSelectedControlActive = true;
						_view.isLockVisible = false;

						break;
					default:
						throw new ArgumentOutOfRangeException(nameof(value), value, null);
				}
			}
		}

		private GarageView _view;

		public void Initialize(GarageView garageView) {
			_view = garageView;
			_view.leftButton.onClick.AddListener(TurnLeftNotify);
			_view.rightButton.onClick.AddListener(TurnRightNotify);
			_view.buyButton.onClick.AddListener(BuyNotify);
			_view.watchVideoButton.onClick.AddListener(WatchNotify);
			_view.selectButton.onClick.AddListener(SelectNotify);
			_view.backButton.onClick.AddListener(BackNotify);
		}

		private void WatchNotify() {
			WatchEvent?.Invoke();
			_uiSounds.PlayClickSound();
		}

		private void BuyNotify() {
			BuyEvent?.Invoke();
			_uiSounds.PlayBuySound();
		}

		private void TurnRightNotify() {
			NextClickedEvent?.Invoke();
			_uiSounds.PlayClickSound();
		}

		private void TurnLeftNotify() {
			PreviousClickedEvent?.Invoke();
			_uiSounds.PlayClickSound();
		}

		private void BackNotify() {
			BackEvent?.Invoke();
			_uiSounds.PlayClickSound();
		}

		private void SelectNotify() {
			ChooseEvent?.Invoke();
			_uiSounds.PlayRepairSound();
		}

		~GaragePresenter() {
			_view.leftButton.onClick.RemoveListener(TurnLeftNotify);
			_view.rightButton.onClick.RemoveListener(TurnRightNotify);
			_view.buyButton.onClick.RemoveListener(BuyNotify);
			_view.watchVideoButton.onClick.RemoveListener(WatchNotify);
			_view.selectButton.onClick.RemoveListener(SelectNotify);
			_view.backButton.onClick.RemoveListener(BackNotify);
		}
	}
}
