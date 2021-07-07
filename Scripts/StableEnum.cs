using System;
using System.Collections.Generic;
using StableEnumUtilities;
using UnityEngine;

[Serializable]
public abstract class StableEnum<T> : BaseStableEnum, IEquatable<StableEnum<T>>, ISerializationCallbackReceiver
                            where T : struct, IFormattable, IConvertible, IComparable
{
    [SerializeField]
    protected T _value;

    protected StableEnum()
    { }

    public T value
    {
        get => _value;
        set => _value = value;
    }

    public override object valueObject => _value;

    public void OnBeforeSerialize()
    {
        _proxy = _value.ToName();
        _index = (int)(ValueType)_value;
    }

    public void OnAfterDeserialize()
    {
        if (!StableEnumHelper.TryParse(_proxy, false, out _value))
        {
            if (Enum.IsDefined(typeof(T), _index))
            {
                _value = (T)(ValueType)_index; // awful boxing, stupid c#  (╯°□°）╯︵ ┻━┻
            }
            else
            {
                Debug.LogErrorFormat("Deserialization failed: \"{0}\" enum has neither \"{1}\" value, nor \"{2}\" index", typeof(T), _proxy, _index);
            }
        }
    }

    public override string ToString() => _value.ToName();
    public bool Equals(StableEnum<T> other) => EqualityComparer<T>.Default.Equals(_value, other.value);
    public override bool Equals(object obj) => Equals(obj as StableEnum<T>);
    public override int GetHashCode() => _value.GetHashCode();
    public static implicit operator T(StableEnum<T> stableEnum) => stableEnum._value;
    public static T Convert(StableEnum<T> stableEnum) => stableEnum._value;
}
