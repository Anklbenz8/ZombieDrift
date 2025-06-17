using System;
using Project;

namespace Gameplay {
    public class LosePresenter {
	    private readonly UiSounds _uiSounds;
	    public event Action RestartEvent, RepairByAdsEvent, RepairByMoneyEvent;
        private LoseView _view;

        public bool isRepairByAdsInteractable {
	        set => _view.isRepairInteractable = value;
        }
        public bool isRepairByMoneyInteractable {
	        set => _view.isRepairByMoneyInteractable = value;
        }

        public int repairCost {
	        set => _view.repairCost = value;
        }
        public LosePresenter(UiSounds uiSounds) {
	        _uiSounds = uiSounds;
        }
        
        public bool enabled {
	        set {
		        if(value)
			        _uiSounds.PlayLoseSound();
		        _view.isActive = value;
	        }
        }

        public void Initialize(LoseView view) {
            _view = view;
            _view.RestartClickedEvent += RestartNotify;
            _view.RepairByAdsClickedEvent += RepairByAdsNotify;
            _view.RepairByMoneyClickedEvent += RepairByMoneyNotify;
        }

        private void RepairByAdsNotify() {
	        _uiSounds.PlayRepairSound();;
	        RepairByAdsEvent?.Invoke();
        }
        
        private void RepairByMoneyNotify() {
	        _uiSounds.PlayRepairSound();;
	        RepairByMoneyEvent?.Invoke();
        }

        private void RestartNotify() {
	        _uiSounds.PlayClickSound();
	        RestartEvent?.Invoke();
        }

        ~LosePresenter() {
            _view.RestartClickedEvent -= RestartNotify;
            _view.RepairByAdsClickedEvent -= RepairByAdsNotify;
            _view.RepairByMoneyClickedEvent -= RepairByMoneyNotify;
        }
    }
}