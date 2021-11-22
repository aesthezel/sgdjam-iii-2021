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
        private Animator[] _animators;
        
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
            _animators = new Animator[_playerLifes.Value];
            
            for (var i = 0; i < _playerLifes.Value; i++)
            {
                 
                _instantiatedGO[i] = Instantiate(_healthIconPrefab, gameObject.transform);
                _animators[i] = _instantiatedGO[i].GetComponent<Animator>();
            }
        }
        
        private void OnPlayerReceiveDamage()
        {
            if(_playerLifes.Value >= 0)
                _animators[_playerLifes.Value].SetTrigger("Hit");
        }
    }
}