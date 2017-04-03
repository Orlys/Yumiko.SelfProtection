
namespace Yumiko.SelfProtection.Verify
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Reflection;

    public class Validator<R>
    {
        private List<MemberInfo> attrs;
        internal Validator()
        {
            var mems = typeof(R)
                .GetMembers((BindingFlags)17301375)
                .Where(x => x.GetCustomAttribute<VerifyAttribute>() != null);
            this.attrs = mems.ToList();
        }

        public void Debug(Action<string> output)
            => attrs.ForEach(x => output($"{x.Name} : {x.MemberType}"));
    }
}
