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

    class Program
    {

        static Program()
        {
            Console.BufferHeight = short.MaxValue - 1;
        }

        static void Main(string[] args)
        {
            main(args);
            Console.ReadKey();
        }

        private static void main(string[] args)
        {
            var bios = Wmi.Get(WmiSubject.PhysicalMedia, Transpiler.Dynamic);
            foreach (var b in bios)
            {
                Console.WriteLine(b.SerialNumber);
                Console.WriteLine(b.Tag);
            }

            return;
            var core = new HydraCore(args[0], new PatchString());
            core.Run();
        }



        public class HydraCore : IDisposable
        {
            private readonly string _file;
            private readonly HydraScript[] _scripts;
            private readonly AssemblyDefinition _assembly;
            public HydraCore(string file, params HydraScript[] scripts)
            {
                this._file = file;
                this._scripts = scripts;
                this._assembly = AssemblyDefinition.ReadAssembly(file);
            }

            public void Run()
            {
                foreach (var script in this._scripts)
                {
                    foreach (var type in this._assembly.MainModule.Types)
                    {
                        foreach (var m in type.Methods)
                        {
                            if(script.Patch(m))
                            {
                                Debug.WriteLine($"Applied script: {script.Name}");
                            }
                        }
                    }
                }
                
            }

            void IDisposable. Dispose()
            {
                this._assembly.Write(this._file);
            }
        }

        public abstract class HydraScript
        {
            public string Name { get; }

            protected HydraScript(string name)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentNullException(nameof(name));
                this.Name = name;
            }

            protected abstract void PatchMethod(MethodDefinition m, ICarried carried);

            protected abstract bool IsMatch(MethodDefinition m, Carried carried);

            public bool Patch(MethodDefinition method)
            {
                var carried = new Carried();
                if(this.IsMatch(method, carried))
                {
                    this.PatchMethod(method, carried);
                    return true;
                }
                return false;
            }
        }


        public interface ICarried
        {
            T Get<T>(string name);
        }
        public sealed class Carried : ICarried
        {
            internal Carried()
            {
                this._carried = new Dictionary<string, object>();
            }
            private readonly Dictionary<string, object> _carried;
            public void Carry(string name, object value)
            {
                lock (this._carried)
                {
                    this._carried[name] = value;
                }
            }

            T ICarried.Get<T>(string name)
            {
                return (T)this._carried[name];
            }
            
        }

        /// <summary>
        /// 輪盤
        /// </summary>
        public class Roulette
        {
            private SecureString _holder;
            public Roulette(SecureString salt)
            {
                this._holder = salt;
            }

            public void Encrypt(string value)
            {

            }
        }

        public class PatchString : HydraScript
        {

            public PatchString():base("Patch string")
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
                    if(a.AttributeType.Name.Equals(NS) &&
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
        


        private delegate bool MatchEvaluate(CustomAttribute attribute);

        /// <summary>
        /// 檢查函式有 <see cref="ProtectedAttribute"/> 標籤
        /// </summary>
        /// <param name="method"></param>
        /// <param name="attribute"></param>
        /// <returns></returns>
        private static bool HasProtectedToken(MethodDefinition method, MatchEvaluate evaluate, out CustomAttribute attribute)
        {
            foreach (var attr in method.CustomAttributes)
            {
                if(evaluate(attr))
                {
                    attribute = attr;
                    return true;
                }
            }
            attribute = null;
            return false;
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

