using System;
using UnityEngine;

[Serializable]
public abstract class BaseStableEnum
{
    public const string kProxyPropName = "_proxy";
    public const string kValuePropName = "_value";

    [SerializeField]
    protected string _proxy;
    public abstract object valueObject { get; }
}
