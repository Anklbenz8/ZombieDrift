using System;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay {
    public class MainMenuView : FadeView {
        public event Action StartGameClickedEvent, GarageClickedEvent, InviteClickedEvent, ShareClickedEvent;

        [SerializeField] private Button _startButton, _garageButton;
        [SerializeField] private Button _shareButton, _inviteButton;
        [SerializeField] private Transform _socialParent;

        public bool socialEnabled {
            set => _socialParent.gameObject.SetActive(value);
            get => _socialParent.gameObject.activeInHierarchy;
        }

        private void OnEnable() {
            _startButton.onClick.AddListener(StartClickedNotify);
            _garageButton.onClick.AddListener(GarageButtonClickedNotify);
          
            _shareButton.onClick.AddListener(ShareNotify);
            _inviteButton.onClick.AddListener(InviteNotify);
        }

        private void OnDisable() {
            _startButton.onClick.RemoveListener(StartClickedNotify);
            _garageButton.onClick.RemoveListener(GarageButtonClickedNotify);
        
            _shareButton.onClick.RemoveListener(ShareNotify);
            _inviteButton.onClick.RemoveListener(InviteNotify);
        }

        private void GarageButtonClickedNotify() =>
            GarageClickedEvent?.Invoke();

        private void StartClickedNotify() =>
            StartGameClickedEvent?.Invoke();

        private void InviteNotify() =>
            InviteClickedEvent?.Invoke();

        private void ShareNotify() =>
            ShareClickedEvent?.Invoke();
    }
}