using System;
using System.Collections.Generic;
using Code.Services;
using Code.Utils;
using UnityEngine;

namespace Code.Patterns
{
    [Serializable]
    public class PoolCandidate
    {
        public string ID;
        public GameObject prefab;
        public int numInstances;
    }
    
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private PoolerDatabase database;
        private Transform _poolerParent;
        private Dictionary<string, List<GameObject>> _dictionary;

        private void Awake()
        {
            _dictionary = new Dictionary<string, List<GameObject>>();
        }

        private void Start()
        {
            _poolerParent = ServiceLocator.Instance.ObtainService<PoolerParentService>().transform;

            foreach (var pair in database.PoolCandidates)
            {
                AddAmount(pair.ID, pair.numInstances, pair.prefab);
            }
        }

        private void AddAmount(string ID, int amount, GameObject prefab)
        {
            if(!_dictionary.ContainsKey(ID))
                _dictionary.Add(ID, new List<GameObject>());
            
            for (int i = 0; i < amount; i++)
            {
                var go = Instantiate(prefab, _poolerParent);
                go.SetActive(false);
                _dictionary[ID].Add(go);
            }
        }

        public GameObject GetByID(string ID)
        {
            if (!_dictionary.ContainsKey(ID)) Debug.LogError("ID not in dictionary");
            var gos = _dictionary[ID];
            
            foreach (var go in gos)
            {
                if (go.activeInHierarchy) continue;
                
                go.SetActive(true);
                return go;
            }
            
            var newGo = Instantiate(_dictionary[ID][0], _poolerParent);
            newGo.SetActive(false);
            _dictionary[ID].Add(newGo);
            return newGo;
        }
    }
}