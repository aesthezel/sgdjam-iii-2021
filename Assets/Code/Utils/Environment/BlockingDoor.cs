using System;
using Code.Data;
using Code.Services;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Code.Utils.Environment
{
    public class BlockingDoor : MonoBehaviour
    {
        [SerializeField] private BoolData keyevent;
        [SerializeField] private UnityEvent events;
        [SerializeField] private UnityEvent endEvents;
        [SerializeField] private SpriteRenderer myrenderer;
        private CameraEffects effects;
        
        private void Start()
        {
            effects = ServiceLocator.Instance.ObtainService<CameraEffects>();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && keyevent.Value)
            {
                myrenderer.sortingOrder = -1;
                events?.Invoke();
                transform.DOMoveY(transform.position.y + 1, 3f).onComplete += () => endEvents?.Invoke();
                effects.DoShake(0.05f, 1f);
            }
        }
    }
}