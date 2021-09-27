using System;
using Code.Camera;
using Code.Services;
using UnityEngine;

public class EventoSecreto : MonoBehaviour
{
    private CameraEffects effects;

    private void Start()
    {
        effects = ServiceLocator.Instance.ObtainService<CameraEffects>();
    }

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
