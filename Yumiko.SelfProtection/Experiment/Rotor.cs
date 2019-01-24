
namespace Yumiko.SelfProtection.Experiment
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading.Tasks;


    [Serializable]
    public sealed class Rotor : ISerializable, IReadOnlyList<byte>
    {
        public static Rotor Random()
        {
            var vt = Enumerable.Range(byte.MinValue, MAX).ToList();
            var g = new Random(Guid.NewGuid().GetHashCode());
            var bytes = new byte[MAX];
            for (var i = default(byte); i < byte.MaxValue; i++)
            {
                var index = g.Next(0, vt.Count);
                var c = vt[index];
                vt.RemoveAt(index);
                bytes[i] = (byte)c;
            }

            bytes[byte.MaxValue] = (byte)vt[0];

            return new Rotor(bytes);
        }


        private const short MAX = 256;

        private readonly static IFormatter s_formatter = new BinaryFormatter();
        public static Rotor FromStream(Stream stream)
        {
            stream.Seek(0, SeekOrigin.Begin);
            return (Rotor)s_formatter.Deserialize(stream);
        }

        private readonly byte[] _r;

        public Rotor(byte[] r)
        {
            if (r.Length != MAX)
                throw new ArgumentOutOfRangeException("The length must be 256");
            this._r = r;
        }

        public byte Shot(byte b)
        {
            return this._r[b];
        }
        private Rotor(SerializationInfo info, StreamingContext context) : this((byte[])info.GetValue("_r", typeof(byte[])))
        {
        }

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(this._r), this._r);
        }

        public void SaveTo(Stream stream)
        {
            s_formatter.Serialize(stream, this);
        }
        public void SaveTo(out Stream stream)
        {
            stream = new MemoryStream();
            s_formatter.Serialize(stream, this);
            stream.Seek(0, SeekOrigin.Begin);
        }

        public byte this[int index]
        {
            get
            {
                return this._r[index];
            }
        }

        public int Count => MAX;

        public IEnumerator<byte> GetEnumerator() => ((IReadOnlyList<byte>)this._r).GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => ((IReadOnlyList<byte>)this._r).GetEnumerator();
    }
}
