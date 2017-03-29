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

            var selfDelete = new ProcessStartInfo("cmd.exe", $"/C ping 1.1.1.1 -n 10 -w 1 > Nul & Taskkill /IM { AppDomain.CurrentDomain.FriendlyName }");
            var DelayDeleter = new ProcessStartInfo("cmd.exe", "/C ping 1.1.1.1 -n 11 -w 1 > Nul & Del \"" + Environment.CurrentDirectory + "\\Installer.exe" + "\"");
            DelayDeleter.WindowStyle = selfDelete.WindowStyle = ProcessWindowStyle.Hidden;
            DelayDeleter.CreateNoWindow = selfDelete.CreateNoWindow = true;
            Process.Start(selfDelete);
            Process.Start(DelayDeleter);

            var cmd = new Dictionary<string, string>
            {
                ["Delay"] = "/C ping 1.1.1.1 -n 10 -w 1 > Nul",
                ["Taskkill"] = $"Taskkill /IM {AppDomain.CurrentDomain.FriendlyName}",
                ["Delete"] = $"Del \"{AppDomain.CurrentDomain.FriendlyName}\""
            };

            cmd["A"] = "";

            //Console.WriteLine(o);
            Console.WriteLine("-------------------------");
            Console.ReadKey();
        }
    }
    
    
}
