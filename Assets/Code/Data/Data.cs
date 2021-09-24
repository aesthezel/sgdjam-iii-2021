using System;
using UnityEngine;
using Sirenix.OdinInspector;

namespace Code.Data
{
    public class Data<T> : ScriptableObject
    {
        public Action OnValueChange;
    
        [SerializeField] private T value;
        [SerializeField] private T startValue;

        private void OnEnable() => value = startValue;
        
        public T Value
        {
            get => value;
            set
            {
                OnValueChange?.Invoke();
                this.value = value;
            }
        }

        public T StartValue => startValue;
    }
}