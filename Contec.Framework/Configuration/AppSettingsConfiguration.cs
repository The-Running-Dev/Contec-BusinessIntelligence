using System.Configuration;
using System.Collections.Generic;

namespace Contec.Framework.Configuration
{
    public class AppSettingsConfiguration : AppSettingsConfiguration<object> { }

    /// <summary>
    /// An implementation of the IConfiguaration interface that pulls setting from the AppSettings
    /// block of the application's configuration file.
    /// </summary>
    public class AppSettingsConfiguration<T> : ConfigurationBase, IConfiguration<T>
    {
        static readonly string _prefix = typeof(T).FullName + ".";

        /// <summary>
        /// Get the requested configuration setting's value from the AppSettings block of the
        /// application's configuration file.
        /// </summary>
        /// <param name="settingName">The name of the setting to retrieve a value for.</param>
        /// <returns>The string value for the requested setting.</returns>
        public override string this[string settingName]
        {
            get
            {
                var val = ConfigurationManager.AppSettings[_prefix + settingName];

                if (val != null)
                {
                    return val;
                }

                return ConfigurationManager.AppSettings[settingName];
            }
        }

        public override IEnumerable<string> Keys
        {
            get { return ConfigurationManager.AppSettings.AllKeys; }
        }
    }
}