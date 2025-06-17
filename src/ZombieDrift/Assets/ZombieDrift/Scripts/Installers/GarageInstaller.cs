using Gameplay;
using Project;
using UnityEngine;
using Zenject;


namespace Garage {
    public class GarageInstaller : MonoInstaller {
        [SerializeField] private EntryPoint _entryPoint;
        [SerializeField] private CarsConfig _carsConfig;

        public override void InstallBindings() {
            InstallEntryPoint();
            InstallGarage();
            InstallStateMachine();
            InstallFactory();
        }

        private void InstallFactory() =>
            Container.Bind<Factory>().AsSingle();

        private void InstallEntryPoint() =>
            Container.BindInterfacesAndSelfTo<EntryPoint>().FromInstance(_entryPoint);

        private void InstallStateMachine() {
            Container.BindInterfacesAndSelfTo<StateSwitcher>().AsSingle();
            Container.Bind<GarageScenario>().AsSingle();
            Container.Bind<InitializeState>().AsSingle();
            Container.Bind<GarageState>().AsSingle();
        }

        private void InstallGarage() {
            Container.Bind<MoneyWallet>().AsSingle();
            Container.Bind<Podium>().AsSingle();
            Container.Bind<ItemsSwitcher>().AsSingle();
            Container.Bind<GaragePresenter>().AsSingle();
            Container.Bind<CarsConfig>().FromInstance(_carsConfig);
        }
    }
}