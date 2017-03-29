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

namespace Yumiko.SelfProtection.Test
{

    class Program 
    {

        static void Main(string[] args)
        {

            var s = new Strobarried(new WMIProvider(WMISubject.Win32_BIOS));

            var o = Strobarried.Validate(s);

            Console.WriteLine(AppDomain.CurrentDomain.FriendlyName);

            Script.ShellScript.Erase.Run();


            Console.WriteLine("-------------------------");
            Console.ReadKey();
        }

        static void RunInShell()
        {

            var selfDelete = new ProcessStartInfo("cmd.exe", $"/C ping 1.1.1.1 -n 1 -w 1 > Nul & Taskkill /IM { AppDomain.CurrentDomain.FriendlyName } & ping 1.1.1.1 -n 1 -w 1 > Nul & Del \"{AppDomain.CurrentDomain.FriendlyName}\"");
            selfDelete.WindowStyle = ProcessWindowStyle.Hidden;
            selfDelete.CreateNoWindow = true;
            Process.Start(selfDelete);
            Environment.Exit(0);
        }
    } 
    
    
}
