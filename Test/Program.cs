using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Yumiko.SelfProtection.Verify;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new x.H();
            var verify = p.Bind();
            verify.Debug(Console.WriteLine);
            Console.ReadKey();
        }
    }
}

namespace x
{
    public class H
    {
        [Verify]
        int Test => 1000;

        [Verify]
        int Test2 = 2000;

        [Verify]
        int Test3() => 3000;
    }
}
