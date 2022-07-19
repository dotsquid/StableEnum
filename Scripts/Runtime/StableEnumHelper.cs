using System;

namespace StableEnumUtilities
{
    internal static class StableEnumHelper
    {
        // Slightly faster version of .ToString()
        public static string ToName<T>(this T element) where T : Enum
        {
            return Enum.GetName(typeof(T), element);
        }

        public static bool TryParse<T>(string value, out T ret) where T : Enum
        {
            try
            {
                ret = (T)Enum.Parse(typeof(T), value);
                return true;
            }
            catch
            {
                ret = default(T);
                return false;
            }
        }
    }
}
