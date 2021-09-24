using Code.Data;
using Code.Interfaces;
using UnityEngine;

namespace Code.Hero
{
    public class GetDamage: MonoBehaviour
    {
        [SerializeField] private IntData playerLifes;

        private void OnTriggerEnter2D(Collider2D other)
        {
            
            IDamageable component = other.GetComponent<IDamageable>();
            
            if (component != null)
            {
                playerLifes.Value -= component.GetDamage();
            }
        }
    }
}