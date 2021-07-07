using System;

namespace StableEnumUtilities
{
    public static class StableEnumHelper
    {
        // Slightly faster version of .ToString()
        public static string ToName<T>(this T element) where T : struct, IFormattable, IConvertible, IComparable
        {
            return Enum.GetName(typeof(T), element);
        }

        public static bool TryParse<T>(string value, bool ignoreCase, out T ret) where T : struct, IFormattable, IConvertible, IComparable
        {
            try
            {
                ret = (T)Enum.Parse(typeof(T), value, ignoreCase);
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
