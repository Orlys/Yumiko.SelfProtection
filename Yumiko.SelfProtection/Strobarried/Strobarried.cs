
namespace Yumiko.SelfProtection.Strobarried
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.CodeDom.Compiler;
    using Microsoft.CSharp;
    using System.Security.Cryptography;

    /// <summary>
    /// 🍓 Strobarried(Strawberry) 🍓
    /// </summary>
    public class Strobarried
    {
        internal readonly HashAlgorithm Hash;
        private CSharpCodeProvider compiler;
        public CompilerParameters Option { get; private set; }
        public IReadOnlyDictionary<string, string> Identifications { get; set; }
        public string Path { get; private set; }
        public Strobarried(IReadOnlyDictionary<string, string> identifications, string path = "Bind.dll", HashAlgorithm hash = null)
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
                OutputAssembly = this.Path = path,
                IncludeDebugInformation = false,
            };
            this.Identifications = identifications;
        }

        public bool Compile()
            => !this.compiler.CompileAssemblyFromSource(this.Option, Encoding.UTF8.GetString(this.x3(this.Identifications))).Errors.HasErrors;
            
        #region x

        private string x0(string rawKey)
            => $"_0x0{ Math.Abs(rawKey.GetHashCode()) }";

        private string x1(string rawValue)
            => string.Join(null, Hash.ComputeHash(Encoding.UTF8.GetBytes(rawValue)));
        
        private byte[] x2(string __, string ___)
            => new byte[]
            {
                0x70, 0x75, 0x62, 0x6C, 0x69, 0x63, 0x20, 0x72, 0x65,
                0x61, 0x64, 0x6F, 0x6E, 0x6C, 0x79, 0x20, 0x73, 0x74,
                0x72, 0x69, 0x6E, 0x67, 0x20
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
                0x53, 0x74, 0x72, 0x6F, 0x62, 0x61, 0x72, 0x72,
                0x69, 0x65, 0x64, 0x7B, 0x75, 0x73, 0x69, 0x6E,
                0x67, 0x20, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D,
                0x3B, 0x69, 0x6E, 0x74, 0x65, 0x72, 0x6E, 0x61,
                0x6C, 0x20, 0x70, 0x61, 0x72, 0x74, 0x69, 0x61,
                0x6C, 0x20, 0x63, 0x6C, 0x61, 0x73, 0x73, 0x20,
                0x42, 0x69, 0x6E, 0x64, 0x3A, 0x4D, 0x61, 0x72,
                0x73, 0x68, 0x61, 0x6C, 0x42, 0x79, 0x52, 0x65,
                0x66, 0x4F, 0x62, 0x6A, 0x65, 0x63, 0x74, 0x7B
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
            => new Random(Guid.NewGuid().GetHashCode()).Next(Math.Abs( this.Identifications.Count + 10),Math.Abs( this.Identifications.Count + 40));
        
        private IEnumerable<byte> x5()
        {
            List<byte[]> b;
            for (b = new List<byte[]>(); b.Count < x6();b.Add(x2((x0(Guid.NewGuid().GetHashCode().ToString())), x1(Guid.NewGuid().GetHashCode().ToString()))));
            return b.SelectMany(x=>x);
        }
        #endregion  
    }
}
