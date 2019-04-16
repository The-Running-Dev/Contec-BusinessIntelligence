using System.Web;
using System.Linq;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;

using Contec.Framework.Extensions;

namespace Contec.Framework.Configuration
{
    public class ContecConfiguration : ContecConfiguration<object> { }

    public class ContecConfiguration<T> : GenericConfiguration, IConfiguration<T>
    {
        private ContecConfiguration(Dictionary<string, string> settings)
        {
            settings.Keys.Each(k => Add(k, settings[k]));
        }

        public ContecConfiguration()
        {
            GetGetConfigSettings();

            //-------------------------------------------------------------------------------------
            // Add it to the configuration registry if we not being called from a subclass.
            if (GetType() == typeof(ContecConfiguration))
            {
                ConfigurationRegistry.AddConfiguration(this); 
            }
        }

        protected override void OnReload()
        {
            GetGetConfigSettings();
        }

        private void GetGetConfigSettings()
        {
            var config = GetConfiguration();

            if (config.HasFile)
            {
                Groups.Add("ConnectionStrings", new ContecConfiguration<T>(GetConnectionStrings(config)));
                Groups.Add("AppSettings", new ContecConfiguration<T>(GetAppSettings(config)));
            }
        }

        private Dictionary<string, string> GetAppSettings(System.Configuration.Configuration config)
        {
            return config.AppSettings.Settings.Cast<KeyValueConfigurationElement>()
                .ToDictionary(elem => elem.Key, elem => elem.Value);
        }

        private Dictionary<string, string> GetConnectionStrings(System.Configuration.Configuration config)
        {
            return config.ConnectionStrings.ConnectionStrings.Cast<ConnectionStringSettings>()
                .ToDictionary(cs => cs.Name, cs => cs.ConnectionString);
        }

        private System.Configuration.Configuration GetConfiguration()
        {
            if (HttpRuntime.AppDomainAppId != null)
            {
                return WebConfigurationManager.OpenWebConfiguration(HttpRuntime.AppDomainAppVirtualPath);
            }

            return ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        }
    }
}