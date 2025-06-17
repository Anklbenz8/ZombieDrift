using System;
using Project;

namespace Gameplay {
    public class GetReadyPresenter {
	    public event Action GoToMenuEvent, GoToPlayEvent;
	    
	    
	    private readonly UiSounds _uiSounds;
        private readonly IInput _input;
        private GetReadyView _view;
        public bool enabled {
	        set => _view.isActive = value;
        }
        public GetReadyPresenter(IInput input, UiSounds uiSounds) {
	        _uiSounds = uiSounds; 
	        _input = input;
        }
        
        public void Initialize(GetReadyView view) {
	        _view = view;
	        _view.BackClickedEvent += GoToMenuNotify;
	        _input.AnyPressedEvent += GoToPlayNotify;
        }
        private void GoToMenuNotify() {
	        _uiSounds.PlayClickSound();
	        GoToMenuEvent?.Invoke();
        }

        private void GoToPlayNotify() =>
            GoToPlayEvent?.Invoke();
        
        ~GetReadyPresenter() {
            _view.BackClickedEvent -= GoToMenuNotify;
            _input.AnyPressedEvent -= GoToPlayNotify;
        }
    }
}