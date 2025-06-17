using SaveLoadSystemNamespace;
using UnityEngine;

namespace Project {
    public class GameSettings : ISaveLoadObject {
        private const string SAVE_KEY = "Settings_Key";
        public string objectKey { get; } = SAVE_KEY;

        public bool isMute {
            get => _gameSettingsData.isMute;
            set => _gameSettingsData.isMute = value;
        }

        private GameSettingsData _gameSettingsData = new();

        public string GetSaveLoadData() =>
            JsonUtility.ToJson(_gameSettingsData);

        public void RestoreValues(string loadData) {
            if (string.IsNullOrEmpty(loadData))
                return;

            _gameSettingsData = JsonUtility.FromJson<GameSettingsData>(loadData);
        }
    }
}