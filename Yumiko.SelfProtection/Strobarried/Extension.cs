
namespace Yumiko.SelfProtection.Core
{
    using System.Collections.Generic;

    public static class Extension
    {
        public static Strobarried ToStrobarried(this IReadOnlyDictionary<string, string> collection)
            => new Strobarried(collection);
    }
}
