using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Collections;
using Yumiko.SelfProtection.WMI;
using System.Security.Cryptography;
using Yumiko.SelfProtection.Strobarried;

namespace Yumiko.SelfProtection.Test
{
    class Program
    {
        static void Main(string[] args)
        {

            Strobarried.Strobarried s = new Strobarried.Strobarried(new WMIProvider(WMISubject.Win32_AccountSID));
            s.Compile();
            Console.ReadKey();   
        }
    }
    
    
}
