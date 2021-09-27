using Code.Data;
using UnityEngine;

namespace Code.Utils.Environment
{
    public class LLaveBehaviour: MonoBehaviour
    {
        [SerializeField] private BoolData keyEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            
            keyEvent.Value = true;
            Destroy(gameObject);
        }
    }
}