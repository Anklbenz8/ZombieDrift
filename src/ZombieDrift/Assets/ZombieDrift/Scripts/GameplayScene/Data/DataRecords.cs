namespace Project {
	public record ProgressData() {
		public int stageIndex;
		public int moneyCount = 100;
		public int currentCarIndex;
		public int[] purchasedCarsIndexes = { 0 };
	}
}