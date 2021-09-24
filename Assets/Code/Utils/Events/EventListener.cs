using UnityEngine;
using UnityEngine.Events;

namespace Code.Utils.Events
{
    public class EventListener : MonoBehaviour
    {
        [SerializeField] private UnityEvent events;
        [SerializeField] private EventStacker stacker;

        private void Awake()
        {
            stacker.AppendListener(events);
        }
    }
}