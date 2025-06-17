using System.Linq;
using UnityEngine;
using SaveLoadSystemNamespace;
using System.Collections.Generic;

namespace Project {
	public class Progress : ISaveLoadObject {
		private const string SAVE_KEY = "progress_key";
		public string objectKey { get; } = SAVE_KEY;
		public HashSet<int> purchasedCars => _purchasedCars;

		// костыль для Socials
		public bool socialsEnabled { set; get; }

		public int comboMultiplier => purchasedCars.Count;
		public int stageIndex {
			get => _progressData.stageIndex;
			set => _progressData.stageIndex = value;
		}
		public int moneyCount {
			get => _progressData.moneyCount;
			set => _progressData.moneyCount = value;
		}
		public int selectedCarIndex {
			get => _progressData.currentCarIndex;
			set => _progressData.currentCarIndex = value;
		}
		private ProgressData _progressData;
		private HashSet<int> _purchasedCars;

		public string GetSaveLoadData() {
			_progressData.purchasedCarsIndexes = _purchasedCars.ToArray();
			return JsonUtility.ToJson(_progressData);
		}
		public void RestoreValues(string loadData) {
			_progressData = string.IsNullOrEmpty(loadData)
					//On first load saveData is empty get first  
					? new ProgressData()
					: JsonUtility.FromJson<ProgressData>(loadData);

			_purchasedCars = new HashSet<int>(_progressData.purchasedCarsIndexes);
		}
	}
}
