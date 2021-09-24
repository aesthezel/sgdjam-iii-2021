using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Code.Utils.Events
{
    [CreateAssetMenu(fileName = "EventStacker", menuName = "Events/EventStacker", order = 0)]
    public class EventStacker : ScriptableObject
    {
        private List<UnityEvent> sceneEvents;
        public Action OnFire;
        
        public void AppendListener(UnityEvent listener) => sceneEvents.Add(listener);

        public void Invoke()
        {
            OnFire?.Invoke();
            foreach (var listener in sceneEvents)
                listener?.Invoke();
        }
        
    }
}