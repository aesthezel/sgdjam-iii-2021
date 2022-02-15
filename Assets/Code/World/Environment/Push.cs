using Code.Hero;
using UnityEngine;

namespace Code.World
{
    public class Push : MonoBehaviour
    {
        [SerializeField] private float pushForce;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                var controller = other.transform.parent.GetComponent<PlayerController2D>();
                controller.PushBack(pushForce);
            }
        }
    }
}