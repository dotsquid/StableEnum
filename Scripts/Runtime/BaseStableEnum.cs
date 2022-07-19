using System;
using UnityEngine;

[Serializable]
public abstract class BaseStableEnum
{
    public const string kValuePropName = "_value";
    public const string kIndexPropName = nameof(_index);
    public const string kProxyPropName = nameof(_proxy);

    [SerializeField]
    protected string _proxy;
    [SerializeField]
    protected int _index;

    public abstract object valueObject { get; }
}
