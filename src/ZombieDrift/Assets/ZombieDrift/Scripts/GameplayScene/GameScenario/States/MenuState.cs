using Ads;
using GamePush;
using Project;

namespace Gameplay {
    public class MenuState : State {
        private const string SHARE_MESSAGE = "Присоединяйся ко мне в игре ЗОМБИ ДРИФТ";

        private readonly StateSwitcher _stateSwitcher;
        private readonly MenuPresenter _menuPresenter;
        private readonly GameplayHudPresenter _gameplayHudPresenter;
        private readonly ScenesLoader _scenesLoader;
        private readonly AdsSystem _adsSystem;
        private readonly Progress _progress;
        private bool _socialsEnabled;

        public MenuState(
            StateSwitcher stateSwitcher,
            MenuPresenter menuPresenter,
            GameplayHudPresenter gameplayHudPresenter,
            ScenesLoader scenesLoader,
            AdsSystem adsSystem,
            Progress progress
        ) : base(stateSwitcher) {
            _stateSwitcher = stateSwitcher;
            _menuPresenter = menuPresenter;
            _gameplayHudPresenter = gameplayHudPresenter;
            _scenesLoader = scenesLoader;
            _adsSystem = adsSystem;

            //Progress only for socials
            _progress = progress;
        }

        public override void Enter() {
            _gameplayHudPresenter.presentState = StagePresentState.StageOnly;
            _menuPresenter.enabled = true;
            _menuPresenter.StartGameEvent += SwitchToPlayState;
            _menuPresenter.GarageEvent += SwitchToGarageState;
#if UNITY_WEBGL
            _menuPresenter.InviteEvent += InviteFriend;
            _menuPresenter.ShareEvent += ShareSocial;

            _socialsEnabled = _progress.socialsEnabled;
            _menuPresenter.socialEnabled = _socialsEnabled;
#endif
        }

        public override void Exit() {
            _gameplayHudPresenter.presentState = StagePresentState.None;
            _menuPresenter.enabled = false;
            _menuPresenter.StartGameEvent -= SwitchToPlayState;
            _menuPresenter.GarageEvent -= SwitchToGarageState;
#if UNITY_WEBGL
            _menuPresenter.InviteEvent -= InviteFriend;
            _menuPresenter.ShareEvent -= ShareSocial;
#endif
        }
#if UNITY_WEBGL
        private void ShareSocial() {
            if (_socialsEnabled)
                GP_Socials.Share(SHARE_MESSAGE, GP_App.Url());
        }

        private void InviteFriend() {
            if (_socialsEnabled)
                GP_Socials.Invite();
        }
#endif
        private void SwitchToGarageState() {
            _adsSystem.ShowFullscreen();
            _scenesLoader.SwitchToShopScene();
        }

        private void SwitchToPlayState() =>
            _stateSwitcher.SetState<GetReadyState>();
    }
}