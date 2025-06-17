namespace Gameplay {
    public class GameplayCache {
        public Map map { get; set; }
        public Car car { get; set; }
        public int mapIndex { get; set; }
        public int mapsCount { get; set; }
        public int comboMultiplier { get; set; }
        public int loseInCurrentStageCount { get; set; }
    }
}