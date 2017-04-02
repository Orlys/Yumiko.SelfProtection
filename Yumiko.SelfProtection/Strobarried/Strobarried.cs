/// Generate Empty DLL
//#define Create_New

/// Display Error In Console 
#define Display_Error

#pragma warning disable 0168
namespace Yumiko.SelfProtection.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.CodeDom.Compiler;
    using Microsoft.CSharp;
    using System.Security.Cryptography;
    using System.IO;

#if !Create_New
    using System.Reflection;
    using Yumiko.SelfProtection.Script;
#endif

    /// <summary>
    /// 🍓🍓🍓 Strobarried (i.e.: Strawberry) 🍀 Powered by NoisserpXeger 🍓🍓🍓
    /// </summary>
    public sealed partial class Strobarried
    {
        #region Declaration
        internal readonly HashAlgorithm Hash;
        private CSharpCodeProvider compiler;
        private CompilerParameters Option { get; set; }
        private IReadOnlyDictionary<string, string> Identifications { get; set; }
        public string FullPath { get; private set; }

        public Strobarried(IReadOnlyDictionary<string, string> identifications, HashAlgorithm hash = null)
        {
            if (identifications == null)
                throw new ArgumentNullException(nameof(identifications));
            this.compiler = new CSharpCodeProvider();
            this.compiler.Supports(GeneratorSupport.AssemblyAttributes);
            this.Hash = hash ?? new SHA512Cng();
            this.Option = new CompilerParameters(new[] { "System.dll", "System.Core.dll" })
            {
                GenerateExecutable = false,
                GenerateInMemory = false,
                IncludeDebugInformation = false,
            };
            this.FullPath = Path.GetFullPath("Bind.dll");
            this.Identifications = identifications;
        }
        private void displayError(CompilerErrorCollection errors)
        {
#if Display_Error
            if (errors.HasErrors)
                errors.Cast<CompilerError>().ToList().ForEach(x => Console.WriteLine(x));
#endif
        }
        private void displayError(string errorMsg)
        {
#if Display_Error
            Console.WriteLine(errorMsg);
#endif
        }
        #endregion

#if Create_New
#line 2
#warning 🍓 You will generate new DLL from this method 🍓
        public bool Compile()
        {
            this.Option.OutputAssembly = this.FullPath;
            var errors = this.compiler.CompileAssemblyFromSource(this.Option, Encoding.UTF8.GetString(x8
            .Concat(x5())
            .Concat(this.Identifications
                .SelectMany(x =>
                    x2(x0(x.Key), x7)))
              .Concat(x5())
            .Concat(new byte[]
            {
                0x7D, 0x7D
            }).ToArray())).Errors;
            this.displayError(errors);
            return !errors.HasErrors;
        }
#line default
#else
        public bool Compile()
        {
            this.Option.OutputAssembly = this.FullPath;
            var errors = this.compiler.CompileAssemblyFromSource(this.Option, Encoding.UTF8.GetString(this.x3(this.Identifications))).Errors;
            this.displayError(errors);
            return !errors.HasErrors;
        }

        public static Evaluation Validate(Strobarried raw)
            => Strobarried.Validate(raw, raw.FullPath);

        /// <summary>
        /// Runtime-binding from path
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="dllPath"></param>
        public static Evaluation Validate(Strobarried raw, string dllPath)
        {
            try
            {
                var t = Assembly.LoadFile(dllPath).GetExportedTypes().SingleOrDefault();
                var o = Activator.CreateInstance(t);
                var fromDll = t.GetMembers()
                                .Where(x => x.MemberType == MemberTypes.Field)
                                .ToDictionary(x => x.Name, x => (x as FieldInfo).GetValue(o).ToString());
                if (fromDll.All(x=>x.Value.Equals(raw.x7)))
                {
                    var origin = raw.FullPath;
                    raw.FullPath = "(ᘡ´・◡・｀)";
                    if (raw.Compile())
                    {
                        var script = new ShellScript(new[] { "Delay", "Terminate", "Delay", "Delete", "SelectDir", "Rename" }, true)
                        {
                            ["Delay"] = "/C ping 1.1.1.1 -n 1 -w 1 > Nul",
                            ["Terminate"] = $"taskkill /IM \"{AppDomain.CurrentDomain.FriendlyName}\"",
                            ["Delete"] = $"del /f \"{origin}\"",
                            ["SelectDir"] = $"cd {Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}",
                            ["Rename"] = $"ren \"{Path.GetFileName(raw.FullPath)}\" \"{Path.GetFileName(origin)}\""
                        };
                        script.Run();
                        return Evaluation.Restart;
                    }
                    else
                        throw new Exception("Access denied");
                }

                var value = string.Empty;
                foreach (var item in raw.Identifications)
                    if (fromDll.TryGetValue(raw.x0(item.Key), out value))
                        if (value == raw.x1(item.Value)) continue;
                        else return  Evaluation.False;
                    else return Evaluation.False;
                return Evaluation.True;
            }
            catch(FileNotFoundException e)
            {
                raw.displayError(e.Message);
                return Evaluation.False;
            }
            catch (Exception e)
            {
                raw.displayError(e.Message);
                return Evaluation.Error;
            }
        }
#endif

        #region Obfuscation
        private string x0(string _)
            => $"_0x0{ Math.Abs(_.GetHashCode()) }";

        private string x1(string _)
            => string.Join(null, Hash.ComputeHash(Encoding.UTF8.GetBytes(_)));

        private byte[] x2(string __, string ___)
            => new byte[]
            {
                0x70, 0x75, 0x62, 0x6C, 0x69, 0x63, 0x20, 0x72,
                0x65, 0x61, 0x64, 0x6F, 0x6E, 0x6C, 0x79, 0x20,
                0x73, 0x74, 0x72, 0x69, 0x6E, 0x67, 0x20
            }
            .Concat(__.Select(_ => (byte)_))
            .Concat(new byte[] { 0x3D, 0x22 })
            .Concat(___.Select(_ => (byte)_))
            .Concat(new byte[] { 0x22, 0x3B }).ToArray();

        private byte[] x3(IReadOnlyDictionary<string, string> parameters)
            => x8
            .Concat(x5())
            .Concat(parameters
                .SelectMany(x =>
                    x2(x0(x.Key), x1(x.Value))))
            .Concat(x5())
            .Concat(new byte[]
            {
                0x7D, 0x7D
            }).ToArray();

        private int x6()
            => new Random(Guid.NewGuid().GetHashCode()).Next(Math.Abs(this.Identifications.Count + 10), Math.Abs(this.Identifications.Count + 40));

        private IEnumerable<byte> x5()
        {
            List<byte[]> b;
            for (b = new List<byte[]>(); b.Count < x6(); b.Add(x2((x0(Guid.NewGuid().GetHashCode().ToString())),
#if Create_New
             x7
#else
             x1(Guid.NewGuid().GetHashCode().ToString())
#endif
             ))) ;
            return b.SelectMany(x => x);
        }

        private string x7 => x1("");

        private byte[] x8 =>
            new byte[]
            {
                0x6E, 0x61, 0x6D, 0x65, 0x73, 0x70, 0x61, 0x63,
                0x65, 0x20, 0x59, 0x75, 0x6D, 0x69, 0x6B, 0x6F,
                0x2E, 0x53, 0x65, 0x6C, 0x66, 0x50, 0x72, 0x6F,
                0x74, 0x65, 0x63, 0x74, 0x69, 0x6F, 0x6E, 0x2E,
                0x43, 0x6F, 0x72, 0x65, 0x7B, 0x75, 0x73, 0x69,
                0x6E, 0x67, 0x20, 0x53, 0x79, 0x73, 0x74, 0x65,
                0x6D, 0x3B, 0x70, 0x75, 0x62, 0x6C, 0x69, 0x63,
                0x20, 0x70, 0x61, 0x72, 0x74, 0x69, 0x61, 0x6C,
                0x20, 0x63, 0x6C, 0x61, 0x73, 0x73, 0x20, 0x42,
                0x69, 0x6E, 0x64, 0x3A, 0x4D, 0x61, 0x72, 0x73,
                0x68, 0x61, 0x6C, 0x42, 0x79, 0x52, 0x65, 0x66,
                0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x7B
            };
        #endregion
    }
}

#pragma warning restore CS0168
