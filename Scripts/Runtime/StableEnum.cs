using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class StableEnum<T> : BaseStableEnum, IEquatable<StableEnum<T>>, ISerializationCallbackReceiver
                            where T : struct, Enum
{
    [SerializeField]
    protected T _value;

    public T value
    {
        get => _value;
        set => _value = value;
    }

    public override object valueObject => _value;

    public bool Equals(StableEnum<T> other) => EqualityComparer<T>.Default.Equals(_value, other.value);
    public override bool Equals(object obj) => Equals(obj as StableEnum<T>);
    public override int GetHashCode() => _value.GetHashCode();
    public static implicit operator T(StableEnum<T> stableEnum) => stableEnum._value;
    public static T Convert(StableEnum<T> stableEnum) => stableEnum._value;

    public void OnBeforeSerialize()
    {
        UpdateProxyAndIndex();
    }

    public void OnAfterDeserialize()
    {
        if (!Enum.TryParse(_proxy, out _value))
        {
            _value = IndexToValue(_index);
            UpdateProxyAndIndex();
        }
    }

    private void UpdateProxyAndIndex()
    {
        _proxy = _value.ToString("F");
        _index = (int)(ValueType)_value;
    }

    private static T IndexToValue(int index)
    {
        int result = 0;
        foreach (var flag in (T[])Enum.GetValues(typeof(T)))
        {
            var flagIndex = (int)(ValueType)flag;
            if ((index & flagIndex) == flagIndex)
            {
                result |= flagIndex;
            }
        }
        return (T)(ValueType)result;
    }
}
