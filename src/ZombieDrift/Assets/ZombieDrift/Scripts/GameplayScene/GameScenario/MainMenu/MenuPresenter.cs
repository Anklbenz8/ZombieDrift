using System;
using Project;
using Zenject;

namespace Gameplay {
    public class MenuPresenter {
        public event Action StartGameEvent, GarageEvent, InviteEvent, ShareEvent;
        private readonly UiSounds _uiSounds;
        private MainMenuView _view;
        private bool _enabled;
        
        public bool enabled {
            set {
                if (value)
                    _view.Appear();
                else
                    _view.Disappear();
            }
        }

        public bool socialEnabled {
            set => _view.socialEnabled = value;
            get => _view.socialEnabled;
        }

        public MenuPresenter(UiSounds uiSounds) =>
            _uiSounds = uiSounds;

        public void Initialize(MainMenuView view) {
            _view = view;
            _view.StartGameClickedEvent += StartGameNotify;
            _view.GarageClickedEvent += GarageClickedNotify;
            _view.ShareClickedEvent += ShareNotify;
            _view.InviteClickedEvent += InviteNotify;
        }

        private void InviteNotify() {
            _uiSounds.PlayClickSound();
            InviteEvent?.Invoke();
        }

        private void ShareNotify() {
            _uiSounds.PlayClickSound();
            ShareEvent?.Invoke();
        }

        private void GarageClickedNotify() {
            _uiSounds.PlayClickSound();
            GarageEvent?.Invoke();
        }

        private void StartGameNotify() =>
            StartGameEvent?.Invoke();

        ~MenuPresenter() {
            _view.StartGameClickedEvent -= StartGameNotify;
            _view.GarageClickedEvent -= GarageClickedNotify;
        }
    }
}