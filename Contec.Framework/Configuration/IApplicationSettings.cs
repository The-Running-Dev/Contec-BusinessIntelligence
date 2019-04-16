namespace Contec.Framework.Configuration
{
    public interface IApplicationSettings
    {
        string AdminUrlRoot { get; }

        string StoreUrlRoot { get; }
    }
}