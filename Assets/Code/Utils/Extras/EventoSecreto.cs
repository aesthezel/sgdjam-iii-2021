using Code.Camera;
using UnityEngine;

public class EventoSecreto : MonoBehaviour
{
    [SerializeField] private CameraEffects effects;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            effects.DoOrtoSize(1f, 1f, Ease.Linear);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            effects.DoOrtoSize(3f, 0.3f, Ease.Linear);
    }
}
