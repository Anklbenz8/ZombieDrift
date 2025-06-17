using Ads;
using SaveLoadSystemNamespace;
using UnityEngine;
using Zenject;

namespace Project {
	public class ProjectInstaller : MonoInstaller {
		[SerializeField] private ProjectConfig _projectConfig;
		[SerializeField] private RootCanvas rootCanvas;
		[SerializeField] private UiSoundConfig _uiSoundConfig;

		public override void InstallBindings() {
			InstallSceneLoader();
		//	InstallProgressService();
			CreateAndInstallRootCanvas();
			InstallProjectEntryPoint();
			InstallCache();
			InstallUiSounds();
			InstallSaveLoadSystem();
			InstallAdsSystem();
			InstallGameSettings();
		}
		private void InstallAdsSystem() {
			Container.Bind<AdsSystem>().AsSingle();
		}
		private void InstallGameSettings() {
			Container.Bind<GameSettings>().AsSingle();
		}
		private void InstallSaveLoadSystem() {
			Container.Bind<SaveLoadSystem>().AsSingle();
		}
		private void InstallUiSounds() {
			Container.Bind<UiSounds>().AsSingle();
			Container.Bind<UiSoundConfig>().FromInstance(_uiSoundConfig);
		}
		private void InstallCache() {
			Container.Bind<Progress>().AsSingle();
		}

		private void CreateAndInstallRootCanvas() {
			var canvas = Instantiate(rootCanvas, this.transform);
			Container.Bind<RootCanvas>().FromInstance(canvas);
		}

		private void InstallProjectEntryPoint() {
			Container.BindInterfacesAndSelfTo<ProjectEntryPoint>().AsSingle().NonLazy();
			Container.Bind<ProjectConfig>().FromInstance(_projectConfig).AsSingle();
		}

		private void InstallSceneLoader() {
			Container.Bind<ScenesLoader>().AsSingle();
		}
	}
}
