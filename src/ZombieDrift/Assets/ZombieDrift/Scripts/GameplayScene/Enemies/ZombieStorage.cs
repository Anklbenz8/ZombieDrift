using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gameplay {
	public class ZombieStorage : IReadOnlyCollection<Zombie> {
		private List<Zombie> _activeZombies;
		private Zombie[] _allZombies;
		//ActiveCount
		public int Count => _activeZombies.Count;
		public void AddNewRange(Zombie[] range) {
			_activeZombies = range.ToList();
			_allZombies = range;
		}

		public bool Deactivate(Zombie item) {
			item.Deactivate(); //?
			return _activeZombies.Remove(item);
		}

		public bool ContainsInActive(Zombie item) =>
				_activeZombies.Contains(item);

		public void DestroyAll() {
			foreach (var zombie in _allZombies)
				UnityEngine.Object.Destroy(zombie.gameObject);

			_allZombies = Array.Empty<Zombie>();
			_activeZombies.Clear();
		}

		public Zombie this[int index] {
			get => _activeZombies[index];
			set => _activeZombies[index] = value;
		}
		public IEnumerator<Zombie> GetEnumerator() =>
				_activeZombies.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() =>
				GetEnumerator();
	}
}
