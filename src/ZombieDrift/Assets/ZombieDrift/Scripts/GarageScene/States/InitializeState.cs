using System.Collections.Generic;
using Gameplay;
using Project;
using UnityEngine;


namespace Garage {
	public class InitializeState : State {
		private readonly StateSwitcher _stateSwitcher;
		private readonly CarsConfig _carsConfig;
		private readonly Progress _progress;
		private readonly Podium _podium;
		private readonly Factory _factory;
		private readonly ItemsSwitcher _itemsSwitcher;

		public InitializeState(
				StateSwitcher stateSwitcher,
				CarsConfig carsConfig,
				Progress progress,
				Podium podium,
				Factory factory,
				ItemsSwitcher itemsSwitcher) : base(stateSwitcher) {
			_stateSwitcher = stateSwitcher;
			_carsConfig = carsConfig;
			_progress = progress;
			_podium = podium;
			_factory = factory;
			_itemsSwitcher = itemsSwitcher;
		}

		public override void Enter() {
			CreateAllModels();
			SwitchToSelectionState();
		}

		private void CreateAllModels() {
			var configCarsArray = _carsConfig.cars;
			var carsCount = configCarsArray.Length;

			var garageItemList = new List<GarageItem>();

			for (var i = 0; i < carsCount; i++) {
			//	var carModel = Object.Instantiate(configCarsArray[i].car.mesh, _podium.spawnParent);
		    	var car = _factory.Create<Car>(configCarsArray[i].carResourcesPath, _podium.spawnParent);
			    car.transform.localPosition =Vector3.zero;
			    Object.Destroy(car);
			    car.body.isKinematic = true;
			    var carModel = car.gameObject;
				carModel.SetActive(false);

				var isCarIsPurchased = _progress.purchasedCars.Contains(i);

				if (!isCarIsPurchased)
					LockCar(carModel);

				var garageData = new GarageItem() {
						mesh = carModel,
						price = configCarsArray[i].price,
						//comboDelay = configCarsArray[i].comboDelay,
						//comboMultiplier = configCarsArray[i].comboMultiplier
				};
				garageItemList.Add(garageData);
			}

			_itemsSwitcher.SetData(garageItemList.ToArray());
		}

		private void LockCar(GameObject car) =>
				Utils.MoveAllChildrenToLayer(car.transform, _carsConfig.lockedLayerMask);

		private void SwitchToSelectionState() =>
				_stateSwitcher.SetState<GarageState>();
	}
}
