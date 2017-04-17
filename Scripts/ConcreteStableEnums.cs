using System;
using UnityEngine;

[Serializable]
public class RuntimePlatformStableEnum : StableEnum<RuntimePlatform>
{
    public static implicit operator RuntimePlatformStableEnum(RuntimePlatform value)
    {
        var stableEnum = new RuntimePlatformStableEnum()
        {
            _value = value
        };
        return stableEnum;
    }
}

[Serializable]
public class SystemLanguageStableEnum : StableEnum<SystemLanguage>
{
    public static implicit operator SystemLanguageStableEnum(SystemLanguage value)
    {
        var stableEnum = new SystemLanguageStableEnum()
        {
            _value = value
        };
        return stableEnum;
    }
}
