namespace Yumiko.SelfProtection.Hydra
{
    public interface ICarried
    {
        T Get<T>(string name);
    }
}
