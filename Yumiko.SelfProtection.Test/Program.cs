using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Collections;
using Yumiko.SelfProtection.WMI;
using System.Security.Cryptography;
using Yumiko.SelfProtection.Core;
using System.IO;
using System.Diagnostics;
using System.Reflection;
using Microsoft.CSharp.RuntimeBinder;
using F = System.IO.File;
namespace Yumiko.SelfProtection.Test
{

    class s
    {

    }

    class ss : Attribute
    { }

    class Program : s
    {
        [ss]
        public static string SelfName => AppDomain.CurrentDomain.FriendlyName;
        public static string Name => Path.GetFileName(SelfName);

        static void Main(string[] args)
        {
            Console.WriteLine(typeof(F));
            Console.ReadKey();
            return;
            var dll = @"C:\Users\user\Documents\visual studio 2015\Projects\Yumiko.SelfProtection\Yumiko.SelfProtection.Test\bin\Debug\Bind.dll";

            var s = new Strobarried(new WMIProvider(WMISubject.Win32_BIOS));

            //var o = Strobarried.Validate(s, dll);
            //Console.WriteLine(o);
            Console.WriteLine("-------------------------");
            Console.ReadKey();
        }
    }
    
    
}
