using Ads;
using Gameplay;
using Project;
using SaveLoadSystemNamespace;
using UnityEngine;

namespace Garage {
    public class GarageState : State {
        private const int WATCH_VIDEO_REWARD_AMOUNT = 200;
        //private const string COMBO_MULTIPLIER_INCREASE_MESSAGE = "Combo +1";

        private readonly ScenesLoader _scenesLoader;
        private readonly Podium _podium;
        private readonly ItemsSwitcher _itemsSwitcher;
        private readonly GaragePresenter _garagePresenter;
        private readonly Progress _progress;
        private readonly MoneyWallet _moneyWallet;
        private readonly SaveLoadSystem _saveLoadSystem;
        private readonly AdsSystem _adsSystem;

        private int selectedCarIndex {
            set => _progress.selectedCarIndex = value;
            get => _progress.selectedCarIndex;
        }

        private GarageItem selectedItem => _itemsSwitcher.selected;
        private int currentIndex => _itemsSwitcher.currentIndex;
        private readonly LayerMask _purchasedLayerMask;

        public GarageState(
            StateSwitcher stateSwitcher,
            ScenesLoader scenesLoader,
            Podium podium,
            ItemsSwitcher itemsSwitcher,
            GaragePresenter garagePresenter,
            Progress progress,
            CarsConfig config,
            MoneyWallet moneyWallet,
            SaveLoadSystem saveLoadSystem,
            AdsSystem adsSystem
        ) : base(stateSwitcher) {
            _purchasedLayerMask = config.purchasedLayerMask;
            _scenesLoader = scenesLoader;
            _podium = podium;
            _itemsSwitcher = itemsSwitcher;
            _garagePresenter = garagePresenter;
            _progress = progress;
            _moneyWallet = moneyWallet;
            _saveLoadSystem = saveLoadSystem;
            _adsSystem = adsSystem;
        }

        public override void Enter() {
            _garagePresenter.NextClickedEvent += ChooseNext;
            _garagePresenter.PreviousClickedEvent += ChoosePrevious;
            _garagePresenter.BackEvent += GotoGameScene;
            _garagePresenter.BuyEvent += BuyCar;
            _garagePresenter.ChooseEvent += Choose;
            _garagePresenter.WatchEvent += WatchRewardVideo;

            _itemsSwitcher.BeforeSelectEvent += Deselect;
            _itemsSwitcher.SelectedChangedEvent += Select;
            _itemsSwitcher.Select(selectedCarIndex);
        }

        public override void Exit() {
            _itemsSwitcher.BeforeSelectEvent -= Deselect;
            _itemsSwitcher.SelectedChangedEvent -= Select;
            _garagePresenter.PreviousClickedEvent -= ChoosePrevious;
            _garagePresenter.NextClickedEvent -= ChooseNext;
            _garagePresenter.BackEvent -= GotoGameScene;
            _garagePresenter.BuyEvent -= BuyCar;
            _garagePresenter.ChooseEvent -= Choose;
            _garagePresenter.WatchEvent -= WatchRewardVideo;
        }

        private async void WatchRewardVideo() {
            var watchingSuccess = await _adsSystem.ShowRewardVideo();
            if (!watchingSuccess)
                return;
            _moneyWallet.AddCoins(WATCH_VIDEO_REWARD_AMOUNT);
            SaveProgress();
            //	_saveLoadSystem.SaveObject(SaveType.PlayerPrefs, _progress);
            RefreshMoneyCount();
        }

        public override void FixedTick() =>
            _podium.RotateAround();

        private void ChooseNext() =>
            _itemsSwitcher.MoveNext();

        private void ChoosePrevious() =>
            _itemsSwitcher.MovePrevious();

        private void Select() {
            selectedItem.mesh.SetActive(true);
            var carPrice = selectedItem.price;

            var isCarPurchased = _progress.purchasedCars.Contains(currentIndex);
            var isChosen = currentIndex == selectedCarIndex;
            _garagePresenter.carPrice = carPrice;

            if (isChosen)
                _garagePresenter.state = GarageItemState.Selected;
            else if (isCarPurchased)
                _garagePresenter.state = GarageItemState.Purchased;
            else if (_moneyWallet.count >= carPrice)
                _garagePresenter.state = GarageItemState.Locked;
            else
                _garagePresenter.state = GarageItemState.NotEnoughMoney;

            RefreshMoneyCount();
            RefreshComboMultiplier();
        }

        private void RefreshComboMultiplier() =>
            _garagePresenter.comboMultiplier = _progress.comboMultiplier;

        private void RefreshMoneyCount() =>
            _garagePresenter.moneyCount = _moneyWallet.count;

        private void Deselect() =>
            selectedItem.mesh.SetActive(false);

        private void BuyCar() {
            var carPrice = selectedItem.price;

            if (_moneyWallet.count < carPrice)
                return;
            _moneyWallet.SpendCoin(carPrice);
            _podium.PlayBuyEffects();

            UnlockCar(selectedItem.mesh);

            _progress.purchasedCars.Add(currentIndex);
            selectedCarIndex = currentIndex;
            Select();
            SaveProgress();
        }

        private void Choose() {
            if (!_progress.purchasedCars.Contains(currentIndex))
                return;
            _podium.PlaySelectEffects();
            selectedCarIndex = currentIndex;
            Select();
            SaveProgress();
        }

        private void SaveProgress() =>
#if !UNITY_EDITOR && UNITY_WEBGL
			_saveLoadSystem.SaveObject(SaveType.GamePushCloud, _progress);
#else
            _saveLoadSystem.SaveObject(SaveType.PlayerPrefs, _progress);
#endif
       //     _saveLoadSystem.SaveObject(SaveType.PlayerPrefs, _progress);

        private void UnlockCar(GameObject car) =>
            Utils.MoveAllChildrenToLayer(car.transform, _purchasedLayerMask);

        private void GotoGameScene() {
            _adsSystem.ShowFullscreen();
            _scenesLoader.SwitchToGameplayScene();
        }
    }
}