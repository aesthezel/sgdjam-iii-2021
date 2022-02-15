using Code.Data;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

namespace Code.World
{
    public class BlockingDoor : MonoBehaviour
    {
        [SerializeField] private BoolData keyevent;
        [SerializeField] private UnityEvent events;
        [SerializeField] private UnityEvent endEvents;
        [SerializeField] private SpriteRenderer myrenderer;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && keyevent.Value)
            {
                myrenderer.sortingOrder = -1;
                events?.Invoke();
                transform.DOMoveY(transform.position.y + 4, 3f).onComplete += () => endEvents?.Invoke();
            }
        }
    }
}