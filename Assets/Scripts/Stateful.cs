using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stateful<T>
{
    [SerializeField] private T _value;
    [SerializeField] private T _beforeValue;
    [SerializeField] private bool _isChanged;

    public delegate void StateUpdate(T value);
    public delegate void ValueChange(T oldValue, T newValue);
    public event StateUpdate OnUpdate;
    public event ValueChange OnValueChange;

    public Stateful(T value)
    {
        _value = value;
        _beforeValue = value;
        _isChanged = false;
    }

    public bool IsChanged {
        get { return _isChanged; }
    }

    public T Value{
        get { return _value; }
        set {
            _beforeValue = _value;
            _value = value;
            _isChanged = !EqualityComparer<T>.Default.Equals(_beforeValue, _value);

            if (_isChanged) {
                if (OnValueChange != null) {
                    OnValueChange(_beforeValue, _value);
                }
            }

            if (OnUpdate != null) {
                OnUpdate(value);
            }
        }
    }
}
