using System;
using Ads;
using Cysharp.Threading.Tasks;
using GamePush;
using GamePush.Initialization;
using SaveLoadSystemNamespace;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using Zenject;
using Object = UnityEngine.Object;

namespace Project {
	public class ProjectEntryPoint : IInitializable {
		private const string POOL_SOUNDS_PARENT_NAME = "SoundsParent";

		private readonly ScenesLoader _scenesLoader;
		private readonly ProjectConfig _config;
		private readonly Progress _progress;
		private readonly UiSounds _uiSounds;
		private readonly SaveLoadSystem _saveLoadSystem;
		private readonly AdsSystem _adsSystem;

		public ProjectEntryPoint(
				ScenesLoader scenesLoader,
				ProjectConfig config,
				Progress progress,
				UiSounds uiSounds,
				SaveLoadSystem saveLoadSystem,
				AdsSystem adsSystem) {
			_scenesLoader = scenesLoader;
			_progress = progress;
			_uiSounds = uiSounds;
			_saveLoadSystem = saveLoadSystem;
			_adsSystem = adsSystem;
			_config = config;
		}

		public void Initialize() {
			Run();
		}

		private async void Run() {
#if UNITY_WEBGL
			GP_Initialization.Execute();
#endif
			_adsSystem.type = AdsType.GamePush;

			CreateLog();
			SetUpProject();
			InitializeUiSounds();

#if UNITY_WEBGL
			while (!GP_Init.isReady) {
				Debug.Log("GP Init not ready");
				await UniTask.Yield();
			}
			Debug.Log("GP Init ready");

			await SetSystemLocale();
			CheckSocialsEnabled();
			TurnOnStickyBanner();
#endif
			await LoadSavedData();
			SwitchToGameplayScene();
		}
#if UNITY_WEBGL
		private void CheckSocialsEnabled() {
			var inviteSupported = GP_Socials.IsSupportsNativeInvite();
			var sharingSupported = GP_Socials.IsSupportsNativeShare();
			Debug.Log($"InviteSupported {inviteSupported} sharingSupported{sharingSupported}");
			_progress.socialsEnabled = inviteSupported && sharingSupported;
		}
#endif
		private void TurnOnStickyBanner()
			=> _adsSystem.ShowStickyBanner();

		private void InitializeUiSounds() {
			var soundsParent = new GameObject(POOL_SOUNDS_PARENT_NAME).transform;
			Object.DontDestroyOnLoad(soundsParent);
			_uiSounds.Initialize(soundsParent);
		}

		private async UniTask SetSystemLocale() {
			await LocalizationSettings.InitializationOperation;

#if UNITY_WEBGL
			SystemLanguage language;
			try {
				language = GP_Language.CurrentSystemLanguage();
			}
			catch (Exception e) {
				language = Application.systemLanguage;
			}
#else
            SystemLanguage language = Application.systemLanguage;
#endif
			var localeIdentifier = new LocaleIdentifier(language);
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.GetLocale(localeIdentifier);
		}

		private async UniTask LoadSavedData() {
#if !UNITY_EDITOR && UNITY_WEBGL
            Debug.Log("Load GamePush -> progress");
			await _saveLoadSystem.LoadObject(SaveType.GamePushCloud, _progress);
#else
			Debug.Log("Load prefs -> prefs");
			await _saveLoadSystem.LoadObject(SaveType.PlayerPrefs, _progress);
#endif
		}

		private void SetUpProject() =>
				Application.targetFrameRate = _config.targetFramerate;

		private void CreateLog() {
			if (_config.isFpsEnabled) {
				var fpsCounter = Object.Instantiate(_config.fpsCounterPrefab);
				Object.DontDestroyOnLoad(fpsCounter);
			}

			if (_config.isDebugEnable) {
				var guiLog = Object.Instantiate(_config.guiLogPrefab);
				Object.DontDestroyOnLoad(guiLog);
			}
		}

		private void SwitchToGameplayScene() =>
				_scenesLoader.SwitchToGameplayScene();
	}
}
