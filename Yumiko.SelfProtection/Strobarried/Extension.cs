
namespace Yumiko.SelfProtection.Core
{
    using System.Collections.Generic;
    using System;

    public static class Extension
    {
        public static Strobarried ToStrobarried(this IReadOnlyDictionary<string, string> collection)
            => new Strobarried(collection);

#if DEBUG
        public static T Debug<T>(this T value)
        {
            Console.WriteLine(value.ToString());
            return value;
        }
#endif
    }
}
