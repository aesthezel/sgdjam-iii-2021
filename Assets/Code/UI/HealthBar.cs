using Code.Hero;
using UnityEngine;

namespace Code.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private GameObject _healthIconPrefab;
        [SerializeField] private PlayerReceiver _playerReceiver;

        private void Start()
        {
            InstancePlayerHealthOnUI();
        }

        private void InstancePlayerHealthOnUI()
        {
            for (int i = 0; i < _playerReceiver.Lifes; i++)
            {
                Instantiate(_healthIconPrefab, gameObject.transform);
            }
        }
        
        // TODO: recibir un evento de PlayerReceiver cuando pierda o gane vida, y asi decida si instanciar otro Prefab o eliminar uno del Parent
        // TODO: tambien podria hacerse un mini Pool System que apague y encienda
    }
}