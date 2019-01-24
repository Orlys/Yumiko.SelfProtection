using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Test
{
    using System.Security.Cryptography;
  //  using System.Reflection;
//    using System.Reflection.Emit;
    using System.IO;
    using System.Reflection;
    using System.Security;

    using Mono.Cecil;
    using Yumiko.SelfProtection;
    using Mono.Cecil.Cil;
    using System.Collections;
    using Yumiko.SelfProtection.Infrastructure;
    using System.Diagnostics;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using Yumiko.SelfProtection.Hydra;

    partial class Program
    {
        static partial void Run(string[] args)
        {
            var uuid = Wmi.Get(WmiSubject.ComputerSystemProduct, Transpiler.Dynamic).First();
            Console.WriteLine(uuid.UUID);
            var bc = Wmi.Get(WmiSubject.BIOS, Transpiler.Dynamic).First();
            Console.WriteLine(bc.BiosCharacteristics as byte[]);

            /*
            using (var m = new MemoryStream())
            using (var c = new CryptoStream(m,
                new AesCryptoServiceProvider
                {
                    KeySize = 256,
                    Key = Encoding.ASCII.GetBytes(new WMIProvider(WMISubject.Win32_BIOS)["BiosCharacteristics"].Replace(", ", null).Substring(0, 32)),
                    IV = Encoding.ASCII.GetBytes(new WMIProvider(WMISubject.Win32_ComputerSystemProduct)["UUID"].Replace("-", null).Substring(16))
                }
                .CreateEncryptor(), CryptoStreamMode.Write))
            {
                var bytes = Encoding.UTF8.GetBytes(raw);
                c.Write(bytes, 0, bytes.Length);
                c.FlushFinalBlock();
                return Convert.ToBase64String(m.ToArray());
            }
            */
        }
    }


    public class PatchString : HydraScript
    {

        public PatchString() : base("Patch string")
        {
            /*
            using (var m = new MemoryStream())
            using (var c = new CryptoStream(m,
                new AesCryptoServiceProvider
                {
                    KeySize = 256,
                    Key = Encoding.ASCII.GetBytes(new WMIProvider(WMISubject.Win32_BIOS)["BiosCharacteristics"].Replace(", ", null).Substring(0, 32)),
                    IV = Encoding.ASCII.GetBytes(new WMIProvider(WMISubject.Win32_ComputerSystemProduct)["UUID"].Replace("-", null).Substring(16))
                }
                .CreateEncryptor(), CryptoStreamMode.Write))
            {
                var bytes = Encoding.UTF8.GetBytes(raw);
                c.Write(bytes, 0, bytes.Length);
                c.FlushFinalBlock();
                return Convert.ToBase64String(m.ToArray());
            }*/
        }
        private const string NS = nameof(ProtectedAttribute);


        protected override bool IsMatch(MethodDefinition m, Carried carried)
        {
            foreach (var a in m.CustomAttributes)
            {
                if (a.AttributeType.Name.Equals(NS) &&
                     m.HasBody &&
                     m.Body.Instructions.Any(inst => inst.OpCode == OpCodes.Ldstr))
                {
                    carried.Carry("cache", a);
                    return true;
                }
            }

            return false;
        }

        protected override void PatchMethod(MethodDefinition m, ICarried carried)
        {
            var attr = carried.Get<CustomAttribute>("cache");

            for (var index = default(int); index < m.Body.Instructions.Count; index++)
            {
                if (m.Body.Instructions[index].OpCode == OpCodes.Ldstr)
                {
                    var origin = m.Body.Instructions[index].Operand as string;

                    //
                    //var encrypted = 
                }
            }

            m.CustomAttributes.Remove(attr);
        }

    }

    public static class Recover
    {
        public static string Get(string s)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(s));
        }
    }





}

