using Code.Interfaces;
using UnityEngine;

namespace Code.World
{
    public class DoesDamage: MonoBehaviour, IDamageable
    {
        [SerializeField] private int damageAmount;
        public int GetDamage() => damageAmount;
    }
}