#pragma warning disable IDE0022
namespace Yumiko.SelfProtection.Kryanbarried
{
    public static class Extension
    {
        public static Validator<R> Bind<R>(this R @object)
            => new Validator<R>();
    }
}
#pragma warning restore IDE0022

