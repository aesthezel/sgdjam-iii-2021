using Code.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Utils.Environment
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class LLaveBehaviour: MonoBehaviour
    {
        [SerializeField] private BoolData keyEvent;
        [SerializeField] private UnityEvent triggerEvents;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            keyEvent.Value = true;
            triggerEvents?.Invoke();
            Destroy(gameObject);
        }
    }
}