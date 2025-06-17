namespace Project {
	public class MoneyWallet {
		private readonly Progress _progress;

		public int count => _progress.moneyCount;

		public MoneyWallet(Progress progress) =>
				_progress = progress;

		public void AddCoins(int value = 1) =>
				_progress.moneyCount += value;

		public void SpendCoin(int value) {
			var moneyCountAfterSpend = _progress.moneyCount - value;
			if (moneyCountAfterSpend > 0)
				_progress.moneyCount = moneyCountAfterSpend;
		}
	}
}
