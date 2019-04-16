namespace Contec.Framework.Configuration
{
    /// <summary>
    /// Supports segregating configuration settings by type.  Typically, the type's namespace-qualified name will be prefixed to the setting name
    /// and then requested from the underlying config getter (such as the one for App.config or the cloud service config), falling back to the setting name
    /// if it's not there.  This allows multiple subtypes of a service to get their own settings from the config file.
    /// </summary>
    /// <typeparam name="T">The type used to segregate configuration settings.  In your service concrete class, have IConfiguration&lt;ConcreteClass&gt; injected to the constructor.</typeparam>
    public interface IConfiguration<T> : IConfiguration
    {
    }
}