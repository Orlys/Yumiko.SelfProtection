namespace Yumiko.SelfProtection.Kryanbarried
{
    using System;
    [AttributeUsage(System.AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public sealed class KryanbarriedAttribute : Attribute { }
}
