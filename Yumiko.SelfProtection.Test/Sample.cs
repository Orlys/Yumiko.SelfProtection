
namespace Yumiko.SelfProtection.Test
{
    using System;
    using Yumiko.SelfProtection.Core;
    using Yumiko.SelfProtection.WMI;

    class Sample 
    {

        static void Main(string[] args)
        {

            var s = new Strobarried(new WMIProvider(WMISubject.Win32_1394Controller));

            var result = Strobarried.Validate(s);
            Console.WriteLine(result);

            Console.WriteLine("-------------------------");
            Console.ReadKey();
        }
        
    } 
    
    
}
