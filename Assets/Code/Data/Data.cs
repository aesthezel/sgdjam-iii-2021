using System;
using UnityEngine;

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
                this.value = value;
                OnValueChange?.Invoke();
            }
        }

        public T StartValue => startValue;
        
        public void ResetValue() => value = startValue;
    }
}