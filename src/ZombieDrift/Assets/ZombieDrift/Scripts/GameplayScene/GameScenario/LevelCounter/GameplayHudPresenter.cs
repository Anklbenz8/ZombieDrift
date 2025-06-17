using System;
using Cysharp.Threading.Tasks;
using Project;
using UnityEngine;
using UnityEngine.Localization;

namespace Gameplay {
    public enum StagePresentState {
        None,
        StageOnly,
        AllWithoutPause,
        All,
        MoneyOnly
    }

    public class GameplayHudPresenter {
        private const string LOCALIZE_TABLE = "StringsTable";
        private const string STAGE_LOCAL_KEY = "stageKey";

        public IGameplayHudActions viewActions => _view;

        public Vector2Int mapIndex {
            set => _view.mapNumber = new Vector2Int(value.x + 1, value.y);
        }

        public int moneyCount {
            set {
                _view.coinsText = value.ToString();
                _moneyCount = value;
            }
        }

 

        private readonly UiSounds _uiSounds;
        private GameplayHudView _view;
        private int _moneyCount;
        private LocalizedString _stageCaptionLocalizedString;

        public GameplayHudPresenter(UiSounds uiSounds) {
            _uiSounds = uiSounds;
            _stageCaptionLocalizedString = new LocalizedString(LOCALIZE_TABLE, STAGE_LOCAL_KEY);
        }

        public void Initialize(GameplayHudView gameplayHudView) {
            _view = gameplayHudView;
            presentState = StagePresentState.None;
        }

        public async UniTask SetStageIndex(int index) {
            var caption = await _stageCaptionLocalizedString.GetLocalizedStringAsync();
            _view.stageCaption = $"{caption} {index + 1}";
        }


        public void IncreaseMoneyCount(int count = 1) {
            moneyCount = _moneyCount + count;
            _uiSounds.PlayCoinSound();
        }

        public StagePresentState presentState {
            set {
                switch (value) {
                    case StagePresentState.None:
                        _view.isActive = false;
                        _view.isMapNumberVisible = false;
                        _view.isPauseButtonVisible = false;
                        _view.isStageVisible = false;
                        break;
                    case StagePresentState.StageOnly:
                        _view.isActive = true;
                        _view.isMapNumberVisible = false;
                        _view.isPauseButtonVisible = false;
                        _view.isStageVisible = true;
                        break;
                    case StagePresentState.AllWithoutPause:
                        _view.isActive = true;
                        _view.isMapNumberVisible = true;
                        _view.isPauseButtonVisible = false;
                        _view.isStageVisible = true;
                        break;
                    case StagePresentState.All:
                        _view.isActive = true;
                        _view.isMapNumberVisible = true;
                        _view.isPauseButtonVisible = true;
                        _view.isStageVisible = true;
                        break;
                    case StagePresentState.MoneyOnly:
                        _view.isActive = true;
                        _view.isMapNumberVisible = false;
                        _view.isPauseButtonVisible = false;
                        _view.isStageVisible = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(value), value, null);
                }
            }
        }
    }
}