using Mono.Cecil;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yumiko.SelfProtection.Hydra
{


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
                        if (script.Patch(m))
                        {
                            Debug.WriteLine($"Applied script: {script.Name}");
                        }
                    }
                }
            }

        }

        void IDisposable.Dispose()
        {
            this._assembly.Write(this._file);
        }
    }
}
