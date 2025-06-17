using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
    public class LoseView : View {
        private const float NOT_INTERACTABLE_ALPHA = 0.35f;
        private const float INTERACTABLE_ALPHA = 0.9f;
        public event Action RestartClickedEvent, RepairByAdsClickedEvent, RepairByMoneyClickedEvent;
        [SerializeField] private Button _repairButton, _restartButton, _repairByMoneyButton;
        [SerializeField] private TMP_Text _repairCostText;
        [SerializeField] private CanvasGroup _repairByAdsCanvasGroup, _repairByMoneyCanvasGroup;

        public bool isRepairInteractable {
            set {
                _repairByAdsCanvasGroup.interactable = value;
                _repairByAdsCanvasGroup.alpha = value
                    ? INTERACTABLE_ALPHA
                    : NOT_INTERACTABLE_ALPHA;
            }
        }

        public bool isRepairByMoneyInteractable {
            set {
                _repairByMoneyCanvasGroup.interactable = value;
                _repairByMoneyCanvasGroup.alpha = value
                    ? INTERACTABLE_ALPHA
                    : NOT_INTERACTABLE_ALPHA;
            }
        }

        public int repairCost {
            set => _repairCostText.text = $"x{value}";
        }

        private void OnEnable() {
            _repairButton.onClick.AddListener(RepairByAdsNotify);
            _restartButton.onClick.AddListener(RestartNotify);
            _repairByMoneyButton.onClick.AddListener(RepairByMoneyNotify);
        }

        private void OnDisable() {
            _repairButton.onClick.RemoveListener(RepairByAdsNotify);
            _restartButton.onClick.RemoveListener(RestartNotify);
            _repairByMoneyButton.onClick.RemoveListener(RepairByMoneyNotify);
        }

        private void RestartNotify() =>
            RestartClickedEvent?.Invoke();

        private void RepairByAdsNotify() =>
            RepairByAdsClickedEvent?.Invoke();

        private void RepairByMoneyNotify() =>
            RepairByMoneyClickedEvent?.Invoke();
    }
}