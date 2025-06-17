using Cysharp.Threading.Tasks;

namespace SaveLoadSystemNamespace {
    public interface ISaveLoadStrategy {
        public void Save(ISaveLoadObject saveLoadData);
        public UniTask Load(ISaveLoadObject saveLoadObject);
    }
}