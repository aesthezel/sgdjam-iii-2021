using System;
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
        private GameObject[] _instantiatedGO;

        private void Awake()
        {
            InstancePlayerHealthOnUI();
        }

        private void Start()
        {
            _playerLifes.OnValueChange += OnPlayerReceiveDamage;
        }

        private void InstancePlayerHealthOnUI()
        {
            _instantiatedGO = new GameObject[_playerLifes.Value];
            
            for (var i = 0; i < _playerLifes.Value; i++)
            {
                _instantiatedGO[i] = Instantiate(_healthIconPrefab, gameObject.transform);
            }
        }
        
        private void OnPlayerReceiveDamage()
        {
            for (var i = 0; i < _instantiatedGO.Length; i++)
            {
                _instantiatedGO[i].SetActive(i < _playerLifes.Value);
            }
        }
    }
}