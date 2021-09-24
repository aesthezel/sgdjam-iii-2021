using System.Collections.Generic;
using Code.Data;
using Code.Hero;
using UnityEngine;

namespace Code.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject _healthIconPrefab;
        [SerializeField] private IntData _playerLifes;
        private GameObject[] instantiatedGO;

        private void Start()
        {
            InstancePlayerHealthOnUI();
            _playerLifes.OnValueChange += OnPlayerReceiveDamage;
        }

        private void InstancePlayerHealthOnUI()
        {
            instantiatedGO = new GameObject[_playerLifes.Value];
            
            for (int i = 0; i < _playerLifes.Value; i++)
            {
                instantiatedGO[i] = Instantiate(_healthIconPrefab, gameObject.transform);
            }
        }
        
        private void OnPlayerReceiveDamage()
        {
            for (int i = 0; i < instantiatedGO.Length; i++)
            { 
                instantiatedGO[i].SetActive(i < _playerLifes.Value);
            }
        }
    }
}