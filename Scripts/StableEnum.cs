using System;
using UnityEngine;

[Serializable]
public abstract class StableEnum<T> : BaseStableEnum, ISerializationCallbackReceiver
    where T : struct, IFormattable, IConvertible, IComparable
{
    [SerializeField]
    protected T _value;

    protected StableEnum()
    {
    }

    public T value
    {
        get { return _value; }
        set { _value = value; }
    }

    public override object valueObject
    {
        get
        {
            return _value;
        }
    }

    public void OnBeforeSerialize()
    {
        _proxy = _value.ToName();
    }

    public void OnAfterDeserialize()
    {
        if (!StableEnumHelper.TryParse(_proxy, false, out _value))
        {
            Debug.LogErrorFormat("Deserialization failed: \"{0}\" enum does not have \"{1}\" value", typeof(T), _proxy);
        }
    }

    public override string ToString()
    {
        return _proxy;
    }

    public static implicit operator T(StableEnum<T> stableEnum)
    {
        return stableEnum._value;
    }
}
