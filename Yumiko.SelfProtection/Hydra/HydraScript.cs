using Mono.Cecil;
using System;

namespace Yumiko.SelfProtection.Hydra
{
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
            if (this.IsMatch(method, carried))
            {
                this.PatchMethod(method, carried);
                return true;
            }
            return false;
        }
    }
}
