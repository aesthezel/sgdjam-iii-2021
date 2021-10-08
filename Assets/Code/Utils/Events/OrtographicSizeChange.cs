using System;
using Code.CameraSystem;
using Code.Data;
using Code.Services;
using UnityEngine;

namespace Code.Utils.Events
{
    public class OrtographicSizeChange : MonoBehaviour
    {
        [SerializeField] private FloatData normalOrthoSize;
        [SerializeField] private float onEnterValue;
        [SerializeField] private float time;
        private CameraEffects effects;
        private float amount;
        
        private void Start()
        {
            effects = ServiceLocator.Instance.ObtainService<CameraEffects>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                amount = onEnterValue - normalOrthoSize.Value;
                effects.DoOrtoSize(amount, time, Ease.Linear);
                normalOrthoSize.Value = onEnterValue;
            }
        }
        
        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                normalOrthoSize.ResetValue();
                effects.DoOrtoSize(amount * -1, time, Ease.Linear);
            }
        }
    }
}