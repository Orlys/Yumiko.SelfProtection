using System.Collections.Generic;

namespace Yumiko.SelfProtection.Hydra
{
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
}
