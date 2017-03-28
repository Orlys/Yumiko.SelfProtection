using System.Collections.Generic;

namespace Yumiko.SelfProtection.Core
{
    public static class Extension
    {
        public static Strobarried ToStrobarried(this IReadOnlyDictionary<string, string> collection)
            => new Strobarried(collection);



    }
}
