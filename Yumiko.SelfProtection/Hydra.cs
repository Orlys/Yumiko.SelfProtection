
namespace Yumiko.SelfProtection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class HydraRecover
    {  

    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ProtectedAttribute : Attribute
    {
        public ProtectedAttribute()
        {
        }
    }
    
}
