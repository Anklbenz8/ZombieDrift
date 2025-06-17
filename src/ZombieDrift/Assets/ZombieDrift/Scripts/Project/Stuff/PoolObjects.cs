using System.Collections.Generic;
using UnityEngine;
using System;

namespace Project {
    public sealed class PoolObjects<T> where T : Component {
        private List<T> _pool;
        private readonly bool _canExpand = false;
        private readonly Transform _parentContainer;
        private readonly T _prefab;

        public PoolObjects(T prefab, int poolAmount, bool canExpand, Transform parentContainer) {
            this._canExpand = canExpand;
            this._parentContainer = parentContainer;
            this._prefab = prefab;

            CreatePool(poolAmount);
        }
        
        public T GetFreeElement() {
            if (HasFreeElement(out var element)) {
                element.gameObject.SetActive(true);
                return element;
            }

            if (_canExpand)
                return CreateElement(true);
            throw new Exception($"â ïóëå çàêîí÷èëèñü {typeof(T)}");
        }

        private void CreatePool(int poolAmount) {
            _pool = new List<T>();

            for (int i = 0; i < poolAmount; i++)
                CreateElement();
        }

        private T CreateElement(bool isActiveAsDefault = false) {
            var createdObj = UnityEngine.Object.Instantiate(_prefab, _parentContainer);
            createdObj.gameObject.SetActive(isActiveAsDefault);
            _pool.Add(createdObj);
            return createdObj;
        }

        private bool HasFreeElement(out T element) {
            element = null;
            foreach (var obj in _pool) {
                if (!obj.gameObject.activeInHierarchy) {
                    element = obj;
                    return true;
                }
            }

            return false;
        }
    }
}