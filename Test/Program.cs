using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Test
{
    using Yumiko.SelfProtection.Kryanbarried;
    using Mono.Cecil;
    using Mono.Cecil.Cil;
    using Yumiko.SelfProtection.Strobarrieds.Core;
    using Yumiko.SelfProtection.WMI;
    using System.Security.Cryptography;
  //  using System.Reflection;
//    using System.Reflection.Emit;
    using System.IO;
    using System.Reflection;

    class Program
    {
        static void E()
        {
            var asm = AssemblyDefinition.ReadAssembly(AppDomain.CurrentDomain.FriendlyName);
            var typs = asm.Modules.SelectMany(m => m.Types);
            var mets = typs.SelectMany(t => t.Methods)
                                .Where(m => m
                                    .CustomAttributes
                                    .Any(a => a.AttributeType.FullName == typeof(KryanbarriedAttribute).FullName));

            var pros = typs.SelectMany(t => t.Properties)
                                .Where(p => p
                                    .CustomAttributes
                                    .Any(a => a.AttributeType.FullName == typeof(KryanbarriedAttribute).FullName));

            var fils = typs.SelectMany(t => t.Fields)
                                .Where(f => f
                                    .CustomAttributes
                                    .Any(a => a.AttributeType.FullName == typeof(KryanbarriedAttribute).FullName));
            
            mets.ToList().ForEach(m =>
            {
                Console.WriteLine(m.FullName);
                foreach (var item in m.Body.Instructions)
                {
                    Console.WriteLine(item.OpCode + "  //" + item.Operand);
                }

            });
            pros.ToList().ForEach(p =>
            {
                Console.WriteLine(p.FullName);
                foreach (var item in p.GetMethod.Body.Instructions)
                {
                    Console.WriteLine(item.OpCode + "  //" + item.Operand);
                }
            });


        }
        static void Stub()
        {

            Console.WriteLine((long)125556);
            Console.WriteLine("Hello world");
        }
        static void Stub2()
        {
            var s = "Hello world";
            Console.WriteLine(s);
            Console.WriteLine(s);
        }
        static void Stub3()
        {
            var s = "Hello";
            Console.WriteLine(s);
            Console.WriteLine(s + "world");
        }




        static void VA(WMISubject sub)
        {
            Strobarried s = new Strobarried(new WMIProvider(sub));
            var v = Strobarried.Validate(s);
            if (v != Strobarried.Evaluation.True)
                Environment.Exit(-1);
        }

        static string enc (string ence)
        {
            var aes = Aes.Create();
            var s = new Strobarried(new WMIProvider(WMISubject.Win32_LogicalDisk));
            aes.Key = null;
            return null;
        }


        class Obfuscator
        {
            private static string encryptString(string raw)
            {
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
            }
            public static void PreEncryptString()
            {
                var asm = AssemblyDefinition.ReadAssembly(AppDomain.CurrentDomain.FriendlyName);
                var m = asm.MainModule;
                var typs = asm.Modules.SelectMany(mod => mod.Types);
                foreach (var method in typs.SelectMany(t => t.Methods))
                {
                    foreach (var target in method.Body.Instructions.Where(x => x.OpCode.Equals(Mono.Cecil.Cil.OpCodes.Ldstr)))
                    {
                        //inject here
                        var b = method.Body;
                        var il = b.GetILProcessor();
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldstr, encryptString(target.Operand as string)));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Stloc_0));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Newobj, m.Import(typeof(MemoryStream).GetConstructor(Type.EmptyTypes))));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Stloc_1));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_1));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Newobj, m.Import(typeof(AesCryptoServiceProvider).GetConstructor(Type.EmptyTypes))));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Dup));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldc_I4, 256));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(SymmetricAlgorithm)).Resolve().Methods.First(x => x.Name == "set_KeySize")));

                        //offset:
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Dup));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Call, m.Import(typeof(Encoding)).Resolve().Methods.First(x => x.Name == "get_ASCII")));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldc_I4_S, 15));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Newobj, m.Import(typeof(WMIProvider).GetConstructor(new Type[1] { typeof(WMISubject) }))));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldstr, "BiosCharacteristics"));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Call, m.Import(typeof(WMIProvider)).Resolve().Methods.First(x => x.Name == "get_Item")));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldstr, ", "));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldnull));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(string)).Resolve().Methods.First(x => x.Name == nameof(string.Replace) & x.Parameters.All(a => a.ParameterType.Equals(m.Import(typeof(string)))))));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldc_I4_0));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldc_I4_S, 32));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(string)).Resolve().Methods.First(x => x.Name == nameof(string.Substring) & x.Parameters.Count == 2)));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(Encoding)).Resolve().Methods.First(x => x.Name == nameof(Encoding.GetBytes) & x.Parameters[0].ParameterType == m.Import(typeof(string)))));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(SymmetricAlgorithm)).Resolve().Methods.First(x => x.Name == "set_Key")));

                        //offset:
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Dup));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Call, m.Import(typeof(Encoding)).Resolve().Methods.First(x => x.Name == "get_ASCII")));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldc_I4_S, 36));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Newobj, m.Import(typeof(WMIProvider).GetConstructor(new Type[1] { typeof(WMISubject) }))));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldstr, "UUID"));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Call, m.Import(typeof(WMIProvider)).Resolve().Methods.First(x => x.Name == "get_Item")));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldstr, "-"));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldnull));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(string)).Resolve().Methods.First(x => x.Name == nameof(string.Replace) & x.Parameters.All(a => a.ParameterType.Equals(m.Import(typeof(string)))))));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldc_I4_S, 16));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(string)).Resolve().Methods.First(x => x.Name == nameof(string.Substring) & x.Parameters.Count == 1)));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(Encoding)).Resolve().Methods.First(x => x.Name == nameof(Encoding.GetBytes) & x.Parameters[0].ParameterType == m.Import(typeof(string)))));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(SymmetricAlgorithm)).Resolve().Methods.First(x => x.Name == "set_IV")));

                        //offset:136
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(SymmetricAlgorithm).GetMethod(nameof(SymmetricAlgorithm.CreateDecryptor)))));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldc_I4_1));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Newobj, m.Import(typeof(CryptoStream).GetConstructor(new[] { typeof(Stream), typeof(ICryptoTransform), typeof(CryptoStreamMode) }))));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Stloc_2));

                        //offset:149
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_0));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Call, m.Import(typeof(Convert).GetMethod(nameof(Convert.FromBase64String)))));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Stloc_3));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_2));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_3));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldc_I4_0));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_3));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldlen));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Conv_I4));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(Stream).GetMethod(nameof(Stream.Write)))));

                        //offset:168
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_2));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(CryptoStream).GetMethod(nameof(CryptoStream.FlushFinalBlock)))));

                        //offset:175
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Call, m.Import(typeof(Encoding)).Resolve().Methods.First(x => x.Name == "get_UTF8")));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_1));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(MemoryStream).GetMethod(nameof(MemoryStream.ToArray)))));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(Encoding)).Resolve().Methods.First(x => x.Name == nameof(Encoding.GetString) & x.Parameters[0].ParameterType.Equals(m.Import(typeof(string))))));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Stloc_0));

                        //offset:193
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        var offset_220 = Instruction.Create(OpCodes.Nop);
                        var offset_207 = Instruction.Create(OpCodes.Leave_S, offset_220);
                        il.InsertBefore(target, Instruction.Create(OpCodes.Leave_S, offset_207));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_2));
                        var offset_206 = Instruction.Create(OpCodes.Endfinally);
                        il.InsertBefore(target, Instruction.Create(OpCodes.Brfalse_S, offset_206));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_2));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(IDisposable).GetMethod(nameof(IDisposable.Dispose)))));

                        //offset:205
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, offset_206);
                        il.InsertBefore(target, offset_207);

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_1));
                        var offset_219 = Instruction.Create(OpCodes.Endfinally);
                        il.InsertBefore(target, Instruction.Create(OpCodes.Brfalse_S, offset_219));

                        il.InsertBefore(target, Instruction.Create(OpCodes.Ldloc_1));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Callvirt, m.Import(typeof(IDisposable).GetMethod(nameof(IDisposable.Dispose)))));

                        //offset:218
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, offset_219);

                        //offset:220
                        il.InsertBefore(target, offset_220);
                        var offset_228 = Instruction.Create(OpCodes.Ldloc_0);
                        il.InsertBefore(target, Instruction.Create(OpCodes.Leave_S, offset_228));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Pop));

                        //offset:224
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));

                        //offset:225
                        il.InsertBefore(target, Instruction.Create(OpCodes.Nop));
                        il.InsertBefore(target, Instruction.Create(OpCodes.Leave_S, offset_228));
                        il.InsertBefore(target, offset_228);

                        il.Remove(target);
                    }
                }
            }
        }
        
        static void ResolveMSIL(string nameof_method_name)
        {
            var asm = AssemblyDefinition.ReadAssembly(AppDomain.CurrentDomain.FriendlyName);
            var typs = asm.Modules.SelectMany(m => m.Types);
            var mets = typs.SelectMany(t => t.Methods).First(x => x.FullName.Contains(nameof_method_name));
            foreach (var item in mets.Body.Instructions)
            {
              
                Console.Write(item.Offset.ToString().PadLeft(3) +" | "+ item.OpCode.ToString().PadRight(12) );
                Console.Write("     " + item.Operand?.GetType() + "   ");
                if (item.Operand is Instruction)
                {

                    var opcode = item.Operand as Instruction;
                    Console.WriteLine();
                    Console.Write("   "+opcode.Offset.ToString().PadLeft(3) + " | " + opcode.OpCode.ToString().PadRight(12));
                    Console.WriteLine( opcode.Operand ?? " // "+opcode.Operand);
                }
                else
                    Console.WriteLine(item.Operand ?? " // " + item.Operand);
            }
        }

        static string EncryptString(string raw)
        {
            using (var m = new MemoryStream())
            using (var c = new CryptoStream(m,
                new AesCryptoServiceProvider
                {
                    KeySize = 256,
                    Key = new WMIProvider(WMISubject.Win32_BIOS)["BiosCharacteristics"].Replace(", ", null).Take(32).Select(x => (byte)x).ToArray(),
                    IV = new WMIProvider(WMISubject.Win32_ComputerSystemProduct)["UUID"].Replace("-", null).Skip(16).Select(x => (byte)x).ToArray()
                }
                .CreateEncryptor(), CryptoStreamMode.Write))
            {
                var bytes = Encoding.UTF8.GetBytes(raw);
                c.Write(bytes, 0, bytes.Length);
                c.FlushFinalBlock();
                return Convert.ToBase64String(m.ToArray());
            }
        }


        static void DecryptString()
        {
            string encryptedRaw = "hhhhh";
            try
            {
                using (var m = new MemoryStream())
                using (var c = new CryptoStream(m,
                    new AesCryptoServiceProvider
                    {
                        KeySize = 256,
                        Key = Encoding.ASCII.GetBytes(new WMIProvider(WMISubject.Win32_BIOS)["BiosCharacteristics"].Replace(", ", null).Substring(0, 32)),
                        IV = Encoding.ASCII.GetBytes(new WMIProvider(WMISubject.Win32_ComputerSystemProduct)["UUID"].Replace("-", null).Substring(16))
                    }
                    .CreateDecryptor(), CryptoStreamMode.Write))
                {
                    var bytes = Convert.FromBase64String(encryptedRaw);
                    c.Write(bytes, 0, bytes.Length);
                    c.FlushFinalBlock();
                    encryptedRaw = Encoding.UTF8.GetString(m.ToArray());
                }
            }
            catch
            {

            }
            var result  = encryptedRaw;
        }

        static void Main(string[] args)
        {
            ResolveMSIL(nameof(DecryptString));
            Console.ReadKey();
            return;

            var asm = AssemblyDefinition.ReadAssembly(AppDomain.CurrentDomain.FriendlyName);
            var g = new GenericInstanceMethod(asm.MainModule.Import(typeof(Enumerable).GetMethod("Take")));
            g.GenericArguments.Add(asm.MainModule.Import(typeof(char)));

            var ins = Instruction.Create(OpCodes.Callvirt,g );
            
            Console.WriteLine(ins.Operand);
            Console.ReadKey();
            return;



            ResolveMSIL(nameof(DecryptString));
            Console.ReadKey();
            return;


            new H().VA();
            Console.ReadKey();
            return;

            var p = new H();
            var verify = p.Bind();
            verify.Debug(Console.WriteLine);
            Console.ReadKey();
        }
    }
}

namespace Test
{
    using System.Reflection;
    using System.Collections;
    using System.Reflection.Emit;
    using Yumiko.SelfProtection.Kryanbarried;
    using Yumiko.SelfProtection.WMI;
    using Yumiko.SelfProtection.Strobarrieds.Core;
    using System.Security.Cryptography;



    public class H
    {
        [Kryanbarried]
        int Test => 1000;

        [Kryanbarried]
        int Test2 = 2000;

        [Kryanbarried]
        int Test3() => 3000;

        [Kryanbarried]
        long Test4() => 4000;


        public void VA()
        {
            var va = new DynamicMethod("#" + Guid.NewGuid(), typeof(void), new Type[1] { typeof(WMISubject) });
            var body = va.GetILGenerator();

            body.DeclareLocal(typeof(Strobarried));
            body.DeclareLocal(typeof(Strobarried.Evaluation));
            body.DeclareLocal(typeof(bool));

            body.Emit(OpCodes.Nop);
            body.Emit(OpCodes.Ldarg_0);
            body.Emit(OpCodes.Newobj, typeof(WMIProvider).GetConstructor(new Type[1] { typeof(WMISubject) }));

            body.Emit(OpCodes.Nop);
            body.Emit(OpCodes.Ldnull);
            body.Emit(OpCodes.Newobj, typeof(Strobarried).GetConstructor(new Type[2] { typeof(IReadOnlyDictionary<string, string>), typeof(HashAlgorithm) }));
            body.Emit(OpCodes.Stloc_0);
            body.Emit(OpCodes.Nop);
            body.Emit(OpCodes.Ldloc_0);
            body.Emit(OpCodes.Call, typeof(Strobarried).GetMethods()
                .Where(m => m.Attributes == (System.Reflection.MethodAttributes.HideBySig |
                                    System.Reflection.MethodAttributes.PrivateScope |
                                    System.Reflection.MethodAttributes.Static |
                                    System.Reflection.MethodAttributes.Public)
                        & m.GetParameters().Count() == 1
                        & m.Name == nameof(Strobarried.Validate))
                .FirstOrDefault());
            body.Emit(OpCodes.Stloc_1);

            body.Emit(OpCodes.Nop);
            body.Emit(OpCodes.Ldloc_1);
            body.Emit(OpCodes.Ldc_I4_2);
            body.Emit(OpCodes.Ceq);
            body.Emit(OpCodes.Ldc_I4_0);
            body.Emit(OpCodes.Ceq);
            body.Emit(OpCodes.Stloc_2);

            body.Emit(OpCodes.Ldloc_2);
            var label = body.DefineLabel();
            body.Emit(OpCodes.Brfalse_S, label);

            body.Emit(OpCodes.Ldc_I4_M1);
            body.Emit(OpCodes.Call, typeof(Environment).GetMethod(nameof(Environment.Exit)));
            body.Emit(OpCodes.Nop);
            body.MarkLabel(label);
            body.Emit(OpCodes.Ret);

            (va.CreateDelegate(typeof(Action<WMISubject>)) as Action<WMISubject>)(WMISubject.Win32_BIOS);
        }
    }
    
}
