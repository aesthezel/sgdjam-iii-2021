using Code.Data;
using Code.Interfaces;
using Code.Utils.Events;
using UnityEngine;

namespace Code.Hero
{
    public class PlayerLiveManager: MonoBehaviour
    {
        [Header("--- LIFE STAT ---")]
        [SerializeField] private IntData playerLifes;

        [Header("--- DYING ---")]
        [SerializeField] private EventStacker onDieEvent;
        
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            
            IDamageable component = other.GetComponent<IDamageable>();
            
            if (component != null)
            {
                playerLifes.Value -= component.GetDamage();
                
                if(playerLifes.Value < 1)
                    onDieEvent.Invoke();
            }
        }
    }
}