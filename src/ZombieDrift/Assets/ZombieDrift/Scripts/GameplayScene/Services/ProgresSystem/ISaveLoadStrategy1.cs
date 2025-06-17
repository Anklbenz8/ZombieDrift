/*
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class SaveLoadData {
	public SaveLoadData(string dataId, object[] data) {
		id = dataId;
	}
	public string id { get; private set; }
	public object[] data { get; private set; }
}

public interface ISaveLoadObject {
	public string componentSaveId { get; }
	public SaveLoadData GetSaveLoadData();
	public void RestoreValues(SaveLoadData loadData);
}

public struct SaveFile {
	public DateTime saveTime { get; }
	public List<SaveLoadData> data { get; }
	public SaveFile(List<SaveLoadData> data) : this() {
		this.data = data;
		saveTime = DateTime.Now;
	}
}

public interface ISaveLoadStrategy {
	public void Save(IEnumerable<ISaveLoadObject> objectsToSave);
	public SaveLoadData[] Load();
}

public class FileSaveLoadStrategy : ISaveLoadStrategy {
	private const string SAVE_FOLDER_NAME = "Saves";
	private const string SAVE_FILE_NAME = "GameSaveFile.json";
	private static string saveDataFolder => Path.Combine(Application.persistentDataPath, SAVE_FOLDER_NAME);
	private static string saveFilePath => Path.Combine(saveDataFolder, SAVE_FILE_NAME);

	public void Save(IEnumerable<ISaveLoadObject> objectsToSave) {
		try {
			var serializedData = objectsToSave.Select(@object => @object.GetSaveLoadData()).ToList();

			if (!Directory.Exists(saveDataFolder))
				Directory.CreateDirectory(saveDataFolder);

			var saveFile = new SaveFile(serializedData);
			var serializedSaveFile = JsonConvert.SerializeObject(saveFile);

			//todo: make async
			File.WriteAllText(saveFilePath, serializedSaveFile);
		}
		catch (Exception e) {
			Debug.LogException(e);
			throw;
		}
	}

	public SaveLoadData[] Load() {
		if (!File.Exists(saveFilePath)) {
			Debug.LogError($"Can't load save file. File {saveFilePath} is doesn't exist.");
			return null;
		}

		try {
			var serializedFile = File.ReadAllText(saveFilePath);

			if (!string.IsNullOrEmpty(serializedFile))
				return JsonConvert.DeserializeObject<SaveFile>(serializedFile).data.ToArray();

			Debug.LogError($"Loaded file {saveFilePath} is empty.");
			return null;
		}
		catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
	}

	public enum SaveType {
		File = 0,
		SteamCloud = 1
	}

	public class SaveLoadSystem {
		private readonly FileSaveLoadStrategy _fileSaveLoadStrategy = new();
		private readonly Dictionary<string, ISaveLoadObject> _componentsIdToSaveObject = new();

		public void RegisterSaveLoadData(ISaveLoadObject saveLoadObject)
			=> _componentsIdToSaveObject[saveLoadObject.componentSaveId] = saveLoadObject;

		public void SaveGame(SaveType saveType) {
			var strategy = saveType switch {
					SaveType.File => _fileSaveLoadStrategy,
					_ => throw new NotImplementedException()
			};
			SaveAll(strategy);
		}

		public void LoadGame(SaveType saveType) {
			var strategy = saveType switch {
					SaveType.File => _fileSaveLoadStrategy,
					_ => throw new NotImplementedException()
			};

			Load(strategy);
		}

		private void SaveAll(ISaveLoadStrategy strategy)
			=> Save(strategy, _componentsIdToSaveObject.Values);

		private void Save(ISaveLoadStrategy strategy, IEnumerable<ISaveLoadObject> data)
			=> strategy.Save(data);

		private void Load(ISaveLoadStrategy strategy) {
			var loadedData = strategy.Load();

			foreach (var data in loadedData) {
				var objectId = data.id;
				if (!_componentsIdToSaveObject.ContainsKey(objectId)) {
					Debug.LogError($"Can't restore data for object with id {objectId}");
					continue;
				}
				_componentsIdToSaveObject[objectId].RestoreValues(data);
			}
		}
	}
}
*/
