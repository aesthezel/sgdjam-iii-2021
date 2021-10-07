using System;
using Code.Camera;
using Code.Services;
using UnityEngine;

public class EventoSecreto : MonoBehaviour
{
    [SerializeField] private float OrthoShize;
    private CameraEffects effects;
    private MainCameraService cam;
    
    private void Start()
    {
        cam = ServiceLocator.Instance.ObtainService<MainCameraService>();
        effects = ServiceLocator.Instance.ObtainService<CameraEffects>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            effects.DoOrtoSize(OrthoShize, 1f, Ease.Linear);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            effects.DoOrtoSize(3f, 0.3f, Ease.Linear);
    }
}
