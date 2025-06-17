using System;
using Gameplay;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
    public interface IGameplayHudActions {
        event Action PauseClickedEvent;
    }

    public class GameplayHudView : View, IFlyingTarget, IGameplayHudActions {
        public event Action PauseClickedEvent;

        [SerializeField] private TMP_Text _stageNumberText;
        [SerializeField] private TMP_Text _mapNumberText;
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private Transform _rewardTarget;
        [SerializeField] private Button _pauseButton;

        public Transform rewardTargetTransform => _rewardTarget;

        public string stageCaption {
            set => _stageNumberText.text = value;
        }

        public Vector2Int mapNumber {
            set => _mapNumberText.text = $"{value.x}/{value.y}";
        }

        public bool isMapNumberVisible {
            set => _mapNumberText.enabled = value;
        }

        public bool isStageVisible {
            set => _stageNumberText.enabled = value;
        }

        public string coinsText {
            set => _coinsText.text = value;
            get => _coinsText.text;
        }

        public bool isPauseButtonVisible {
            set => _pauseButton.gameObject.SetActive(value);
        }

        private void PauseNotify() =>
            PauseClickedEvent?.Invoke();

        private void OnEnable() =>
            _pauseButton.onClick.AddListener(PauseNotify);

        private void OnDisable() =>
            _pauseButton.onClick.RemoveListener(PauseNotify);
    }
}