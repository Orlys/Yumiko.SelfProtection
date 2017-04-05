using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yumiko.SelfProtection.Kryanbarried;
using Mono.Cecil;
using Mono.Cecil.Cil;
using Yumiko.SelfProtection.Strobarrieds.Core;
using Yumiko.SelfProtection.WMI;

namespace Test
{
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
        static void VA()
        {
            Strobarried s = new Strobarried(new WMIProvider(WMISubject.Win32_BIOS));
            var v = Strobarried.Validate(s);
            if (v != Strobarried.Evaluation.True)
                Environment.Exit(-1);
        }

        static string MSIL_To_CSharpCode(string path , string methodName)
        {
            var builder = new StringBuilder();
            var asm = AssemblyDefinition.ReadAssembly(path);
            var typs = asm.Modules.SelectMany(m => m.Types);
            var mets = typs.SelectMany(t => t.Methods).Where(t => t.FullName.Contains(methodName));
            foreach (var method in mets)
            {
                var fullname = $"<Method '{method.FullName}'>";
                builder.AppendLine(fullname);
                var methodDefinition = $"<MethodDefinition '{nameof(mets)}'>";
                builder.AppendLine(methodDefinition);
                var ilpcsr = $"<ILProcessor '{"ilpcsr"}'>";
                builder.AppendLine(ilpcsr);
                foreach (var instruction in method.Body.Instructions)
                {
                    if (instruction.Operand == null)
                    {

                    }
                    else if (instruction.Operand is byte |
                            instruction.Operand is char |
                            instruction.Operand is short |
                            instruction.Operand is ushort |
                            instruction.Operand is int |
                            instruction.Operand is uint |
                            instruction.Operand is long |
                            instruction.Operand is ulong |
                            instruction.Operand is float |
                            instruction.Operand is double |
                            instruction.Operand is string)
                    {

                    }
                    else if (instruction.Operand is MethodReference)
                    {

                    }
                    else if (instruction.Operand is FieldReference)
                    {

                    }
                    else if (instruction.Operand is PropertyReference)
                    {

                    }
                    else if (instruction.Operand is Instruction)
                    {
                        //  method.Body.GetILProcessor().Create()
                    }
                    else
                    {
                        throw new Exception($"Unhandled type : {instruction.GetType()}");
                    }
                    
                    var opcode = $"Instruction.Create(OpCodes.{instruction.OpCode.Name.Replace(".", "_")})";
                 
                }
            }

            return builder.ToString();
        }

        static void E2()
        {
            var asm = AssemblyDefinition.ReadAssembly(AppDomain.CurrentDomain.FriendlyName);
            var typs = asm.Modules.SelectMany(m => m.Types);
            var mets = typs.SelectMany(t => t.Methods).First(x => x.FullName.Contains("VA"));
            

            /*
            var ldstr = Instruction.Create(OpCodes.Ldstr, "Hello");
            var ilpcsr = mets.Body.GetILProcessor();
            ilpcsr.InsertAfter(mets.Body.Instructions[0], ldstr);
            var writeline = asm.MainModule.Import(typeof(Console).GetMethod(nameof(Console.WriteLine), new Type[] { typeof(string) }));
            ilpcsr.InsertAfter(ldstr, Instruction.Create(OpCodes.Call, writeline));
            var p = AppDomain.CurrentDomain.FriendlyName.Replace(".exe", "Patch.exe");
            asm.Write(p);

            var asm2 = AssemblyDefinition.ReadAssembly(p);
            var typs2 = asm2.Modules.SelectMany(m => m.Types);
             mets = typs2.SelectMany(t => t.Methods).First(x => x.FullName.Contains("VA"));
            */
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
            Console.ReadKey();
            VA();
            Console.ReadKey();
            return;
            E();

            Console.ReadKey();
            return;


            var p = new x.H();
            var verify = p.Bind();
            verify.Debug(Console.WriteLine);
            Console.ReadKey();
        }
    }
}

namespace x
{
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
    }
}
