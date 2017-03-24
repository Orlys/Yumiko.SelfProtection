using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Collections;
using Yumiko.SelfProtection.WMI;

namespace Yumiko.SelfProtection.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var wmi = new WMIProvider(WMISubject.Win32_BIOS);
            foreach (var item in wmi)
            {
                Console.WriteLine(item);
            }
            Console.ReadKey();   
        }
    }
}
