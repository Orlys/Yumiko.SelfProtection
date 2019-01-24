
namespace Yumiko.SelfProtection.Infrastructure
{

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Management;
    using Yumiko.SelfProtection.Infrastructure.Contract;

    /// <summary>
    /// Gets the system info, see https://docs.microsoft.com/en-us/windows/desktop/cimwin32prov/computer-system-hardware-classes
    /// </summary>
    public static class Wmi
    {

        private static object Convert(CimType cimType, object value, bool isArray)
        {
            switch (cimType)
            {
                case CimType.SInt8: return Conv<sbyte>(value, isArray);
                case CimType.UInt8: return Conv<byte>(value, isArray);
                case CimType.SInt16: return Conv<short>(value, isArray);
                case CimType.UInt16: return Conv<ushort>(value, isArray);
                case CimType.SInt32: return Conv<int>(value, isArray);
                case CimType.UInt32: return Conv<uint>(value, isArray);
                case CimType.SInt64: return Conv<long>(value, isArray);
                case CimType.UInt64: return Conv<ulong>(value, isArray);
                case CimType.Real32: return Conv<decimal>(value, isArray);
                case CimType.Real64: return Conv<decimal>(value, isArray);
                case CimType.Boolean: return Conv<bool>(value, isArray);
                case CimType.String: return Conv<string>(value, isArray);
                case CimType.Char16: return Conv<char>(value, isArray);

                case CimType.Reference: return Conv<short>(value, isArray);
                case CimType.DateTime: return ConvDT(value, isArray);

                default: return Conv<object>(value, isArray);
            }

        }

        private static object ConvDT(object value, bool isArray)
        {
            if (value != null)
            {
                if (isArray)
                {
                    var list = new List<DateTime>();
                    foreach (var item in value as IEnumerable)
                        list.Add(ToDateTime(item.ToString()));
                    return list.ToArray();
                }
                return ToDateTime(value.ToString());
            }
            else if (isArray)
                return new DateTime[0];
            return default(DateTime);

            DateTime ToDateTime(string fmt)
            {
                return DateTime.ParseExact(fmt,
                       "yyyyMMddHHmmss.ffffff+000",
                       CultureInfo.InvariantCulture);
            }
        }

        private static object Conv<T>(object value, bool isArray)
        {
            if (value is T t) return t;
            else if (value is T[] ts)
            {
                return ts;
            }
            else if (isArray) return new T[0];
            return string.Empty;
        }
        
        public static IEnumerable<T> Get<T>(WmiSubject subject, ITranspiler<T> transpiler, bool localOnly = true)
        {
            var seacher = new ManagementObjectSearcher($"select * from Win32_{subject.ToString().TrimStart('_')}");

            foreach (ManagementObject mo in seacher.Get())
            {
                var dict = transpiler.Create();
                try
                {
                    foreach (var pd in mo.Properties)
                    {
                        if ((localOnly && pd.IsLocal) || !localOnly)
                        {
                            var name = pd.Name;
                            var value = Convert(pd.Type, pd.Value, pd.IsArray);
                            dict.Add(name, value);
                        }
                        
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                    continue;
                }
                yield return transpiler.Convert(dict);
            }
        }


    }
}
