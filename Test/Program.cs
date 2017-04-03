using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Yumiko.SelfProtection.Verify;
using Mono.Cecil;
using Mono.Cecil.Cil;

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
                                    .Any(a => a.AttributeType.FullName == typeof(VerifyAttribute).FullName));

            var pros = typs.SelectMany(t => t.Properties)
                                .Where(p => p
                                    .CustomAttributes
                                    .Any(a => a.AttributeType.FullName == typeof(VerifyAttribute).FullName));

            var fils = typs.SelectMany(t => t.Fields)
                                .Where(f => f
                                    .CustomAttributes
                                    .Any(a => a.AttributeType.FullName == typeof(VerifyAttribute).FullName));
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

        static void Main(string[] args)
        {

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
        [Verify]
        int Test => 1000;

        [Verify]
        int Test2 = 2000;

        [Verify]
        int Test3() => 3000;

        [Verify]
        long Test4() => 4000;
    }
}
