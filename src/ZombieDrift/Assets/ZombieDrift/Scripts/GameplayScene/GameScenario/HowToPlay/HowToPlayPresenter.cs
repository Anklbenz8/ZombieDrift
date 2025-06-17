using Cysharp.Threading.Tasks;

namespace Gameplay {
    public class HowToPlayPresenter {
        private const int HIDE_DELAY_MILLISECONDS = 1500;
        private HowToPlayView _view;

        public bool enabled {
            set {
                if (value)
                    _view.Appear(0);
                else
                    _view.Disappear(0);
            }
        }

        public void Initialize(HowToPlayView view) =>
            _view = view;

        public async void HideWithDelay() {
            await UniTask.Delay(HIDE_DELAY_MILLISECONDS);

            _view.Disappear();
        }
    }
}