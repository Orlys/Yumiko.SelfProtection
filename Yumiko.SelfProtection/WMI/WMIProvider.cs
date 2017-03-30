namespace Yumiko.SelfProtection.WMI
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Management;
    using System.Collections;

    public class WMIProvider : IReadOnlyDictionary<string, string>
    {
        #region IReadOnlyDictionary<string,string>

        private IDictionary<string, string> wmi;

        public string this[string key]
            => this.wmi[key];

        public int Count
            => this.wmi.Count;

        public IEnumerable<string> Keys
            => this.wmi.Keys;

        public IEnumerable<string> Values
            => this.Values;

        public bool ContainsKey(string key)
            => this.wmi.ContainsKey(key);

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
            => this.wmi?.GetEnumerator();

        public bool TryGetValue(string key, out string value)
            => this.wmi.TryGetValue(key, out value);

        IEnumerator IEnumerable.GetEnumerator()
            => this.GetEnumerator();

        #endregion

        public WMISubject Subject { get; private set; }

        internal virtual void SetSubject(WMISubject subject)
            => this.wmi = new ManagementObjectSearcher($"select * from { subject}")
                        .Get()
                        .Cast<ManagementObject>()
                        ?.FirstOrDefault()
                        .Properties
                        .Cast<PropertyData>()
                        .Where(x => x.Value != null)
                        .Aggregate(new Dictionary<string, string>(), (x, item) =>
                        {
                            x.Add(item.Name, (item.Value is IEnumerable & !(item.Value is string))
                                ? $"{string.Join(", ", (item.Value as IEnumerable).Cast<object>())}"
                                : item.Value.ToString());
                            return x;
                        });

        public WMIProvider(WMISubject subject)
        {
            this.SetSubject(this.Subject = subject);
        }
    }
}
