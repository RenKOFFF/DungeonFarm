using System;
using UnityEngine.Events;

namespace Base.Observable
{
    public class ObservableVariable<T>
    {
        public event Action<T> OnChanged;

        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnChanged?.Invoke(_value);
            }
        }

        public ObservableVariable() => Value = default;
        
        public ObservableVariable(T defaultValue)
        {
            Value = defaultValue;
        }

        public override string ToString()
        {
            return _value.ToString();
        }
    }
}