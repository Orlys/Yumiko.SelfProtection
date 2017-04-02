namespace Yumiko.SelfProtection.Verify
{
    public static class ValidateExtension
    {
        public static Validator<R> Bind<R>(this R @object)
            => new Validator<R>();
    }
}
