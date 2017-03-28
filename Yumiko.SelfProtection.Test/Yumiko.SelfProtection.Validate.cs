using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yumiko.SelfProtection.Validate
{
    [AttributeUsage(AttributeTargets.Method , Inherited = false)]
    public class ValidateAttribute : Attribute
    {

    }
}
