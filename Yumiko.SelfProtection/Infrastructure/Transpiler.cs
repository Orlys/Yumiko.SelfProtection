
namespace Yumiko.SelfProtection.Infrastructure
{
    using System.Collections.Generic;
    using System.Dynamic;
    using Yumiko.SelfProtection.Infrastructure.Contract;
    using Yumiko.SelfProtection.Infrastructure.Internal;

    public static class Transpiler
    {
        public readonly static ITranspiler<dynamic> Dynamic
            = new Transpiler<dynamic>(() => new ExpandoObject());

        public readonly static ITranspiler<IReadOnlyDictionary<string, object>> ReadOnly 
            = new Transpiler<IReadOnlyDictionary<string, object>>(() => new Dictionary<string, object>());
    }
}
