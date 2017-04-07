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
        static void VA(WMISubject sub)
        {
            Strobarried s = new Strobarried(new WMIProvider(sub));
            var v = Strobarried.Validate(s);
            if (v != Strobarried.Evaluation.True)
                Environment.Exit(-1);
        }


        static void E2()
        {
            var asm = AssemblyDefinition.ReadAssembly(AppDomain.CurrentDomain.FriendlyName);
            var typs = asm.Modules.SelectMany(m => m.Types);
            var mets = typs.SelectMany(t => t.Methods).First(x => x.FullName.Contains("VA"));
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
        static void Main(string[] args)
        {
            E2();
            Console.WriteLine(  "================================");
            new H().TestInject();

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

        delegate void Remote(WMISubject sub);
        public void TestInject()
        {
            var va = new DynamicMethod("va", typeof(void), new Type[1] { typeof(WMISubject) });
            var body = va.GetILGenerator();

            body.Emit(OpCodes.Nop);
            //new WMIProvider([WMISubject]arg0);
            body.Emit(OpCodes.Ldarg_0); 
            body.Emit(OpCodes.Newobj, typeof(WMIProvider).GetConstructor(new Type[1] { typeof(WMISubject) }));

            body.Emit(OpCodes.Ldnull);
            body.Emit(OpCodes.Newobj, typeof(Strobarried).GetConstructor(new Type[2] { typeof(IReadOnlyDictionary<string, string>), typeof(HashAlgorithm) }));
            body.Emit(OpCodes.Stloc_0);

            body.Emit(OpCodes.Ldloc_0);
            body.Emit(OpCodes.Call, typeof(Strobarried).GetMethods()
                .Where(m => m.Attributes == (
                        System.Reflection.MethodAttributes.HideBySig |
                        System.Reflection.MethodAttributes.PrivateScope |
                        System.Reflection.MethodAttributes.Static |
                        System.Reflection.MethodAttributes.Public)
                        & m.GetParameters().Count() == 1)
                .FirstOrDefault());
            body.Emit(OpCodes.Stloc_1);

            body.Emit(OpCodes.Ldloc_1);
            body.Emit(OpCodes.Ldc_I4_2);
            body.Emit(OpCodes.Ceq);

            body.Emit(OpCodes.Ldc_I4_0);
            body.Emit(OpCodes.Ceq);
            body.Emit(OpCodes.Stloc_2);

            var label = body.DefineLabel();
            body.Emit(OpCodes.Ldloc_2);
            body.Emit(OpCodes.Brfalse_S,label);
            body.Emit(OpCodes.Ldc_I4_0);
            body.Emit(OpCodes.Call, typeof(Environment).GetMethod(nameof(Environment.Exit)));

            body.MarkLabel(label);
            body.Emit(OpCodes.Nop);
            body.Emit(OpCodes.Ret);


            var o = (Remote)va.CreateDelegate(typeof(Remote));
            o(WMISubject.Win32_BIOS);

            
        }
    }
}
