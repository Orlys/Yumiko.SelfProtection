/// Display Error In Console 
//#define Display_Error

/// Mode Switcher
//#define DynaLoading

#pragma warning disable CS0168
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
    using System.Threading;

#if DynaLoading
    using System.Reflection;
#else
    using Token = Core.Bind;
    using Microsoft.CSharp.RuntimeBinder;
#endif

    /// <summary>
    /// 🍓🍓🍓 Strobarried (i.e.: Strawberry) / Powered by Viyrex 🍓🍓🍓
    /// </summary>
    public class Strobarried
    {
        internal readonly HashAlgorithm Hash;
        private CSharpCodeProvider compiler;
        private CompilerParameters Option { get; set; }
        private IReadOnlyDictionary<string, string> Identifications { get; set; }
        public string FullPath { get; private set; }

        public Strobarried(IReadOnlyDictionary<string, string> identifications, string path = null, HashAlgorithm hash = null)
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
                OutputAssembly = path ?? "Bind.dll",
                IncludeDebugInformation = false,
            };
            this.FullPath = Path.GetFullPath(this.Option.OutputAssembly);
            this.Identifications = identifications;
        }
        
        public bool Compile()
        {
            var errors = this.compiler.CompileAssemblyFromSource(this.Option, Encoding.UTF8.GetString(this.x3(this.Identifications))).Errors;

#if Display_Error
            if (errors.HasErrors)
                foreach (var item in errors)
                    Console.WriteLine(item);
#endif
            return !errors.HasErrors;
        }


#if DynaLoading

        /// <summary>
        /// Dynamic-binding from path
        /// </summary>
        /// <param name="raw"></param>
        /// <param name="dllPath"></param>
        /// <returns></returns>
        public static bool Validate(Strobarried raw, string dllPath)
        {
            try
            {
                var t = Assembly.LoadFile(dllPath).GetExportedTypes().SingleOrDefault();
                var o = Activator.CreateInstance(t);
                var fromDll = t.GetMembers()
                                .Where(x => x.MemberType == MemberTypes.Field)
                                .ToDictionary(x => x.Name, x => (x as FieldInfo).GetValue(o).ToString());

                var value = string.Empty;
                foreach (var item in raw.Identifications)
                    if (fromDll.TryGetValue(raw.x0(item.Key), out value))
                        if (value == raw.x1(item.Value)) continue;
                        else return false;
                    else return false;
                return true;
            }
            catch (Exception e)
            {
#if Display_Error
                Console.WriteLine(e.Message);
#endif
                return false;
            }
        }
#else
        /// <summary>
        /// Pre-binding via reference dll
        /// </summary>
        /// <returns></returns>
        public static bool Validate(Strobarried raw)
        {
            dynamic token = new Token();
            try
            {
                if (token.Will_Be_Remove == 0xFF)
                {
                    //todo : Need Generate DLL and restart application


                }
            }
            catch (RuntimeBinderException e) when (e.Message.Contains(nameof(token.Will_Be_Remove)))
            {
                var t = typeof(Token);
                var d = t.GetFields().Select(x => new { x.Name, Value = x.GetValue(token as Token).ToString() }).ToDictionary(x => x.Name, x => x.Value);

                var value = string.Empty;
                foreach (var item in raw.Identifications)
                    if (d.TryGetValue(raw.x0(item.Key), out value))
                        if (value == raw.x1(item.Value)) continue;
                        else return false;
                    else return false;
                return true;
            }
            catch(Exception e)
            {
#if Display_Error
                Console.WriteLine(e.Message);
#endif
            }
            return false;
        }
#endif

        #region x
        private string x0(string rawKey)
            => $"_0x0{ Math.Abs(rawKey.GetHashCode()) }";

        private string x1(string rawValue)
            => string.Join(null, Hash.ComputeHash(Encoding.UTF8.GetBytes(rawValue)));

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
            => new byte[]
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
            }
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
            for (b = new List<byte[]>(); b.Count < x6(); b.Add(x2((x0(Guid.NewGuid().GetHashCode().ToString())), x1(Guid.NewGuid().GetHashCode().ToString())))) ;
            return b.SelectMany(x => x);
        }


        #endregion
    }
}

#pragma warning restore CS0168
